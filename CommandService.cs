using System.Collections.ObjectModel;
using System.Reflection;
using Masya.Telegram.Commands.Abstractions;
using Masya.Telegram.Commands.Attributes;
using Masya.Telegram.Commands.Exceptions;
using Masya.Telegram.Commands.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Masya.Telegram.Commands
{
    public delegate Task LogEventHandler(Log log);

    public class CommandService : ICommandService
    {
        private IReadOnlyCollection<ModuleInfo>? _modules;
        private IReadOnlyDictionary<string, CommandInfo>? _commands;

        public ITelegramBotClient Client { get; }
        public IServiceProvider Services { get; }
        public bool ThrowOnUnknownCommand { get; }

        public event LogEventHandler? OnLog;

        public const string ArgsSeparator = " ";

        public CommandService(ITelegramBotClient client, IServiceProvider services, bool throwOnUnknownCommand = false)
        {
            Client = client;
            Services = services;
            ThrowOnUnknownCommand = throwOnUnknownCommand;
        }

        public Task AddModulesAsync(Assembly assembly)
        {
            return Task.Run(() =>
            {
                var validModuleTypes = new List<ModuleInfo>();
                var validCommands = new Dictionary<string, CommandInfo>();
                int totalCommands = 0;

                foreach (var type in assembly.DefinedTypes)
                {
                    if (IsValidModuleType(type))
                    {
                        var commands = new Dictionary<string, CommandInfo>();
                        foreach (var method in type.DeclaredMethods)
                        {
                            if (IsValidCommandHandler(method))
                            {
                                try
                                {
                                    var commandInfo = MakeCommandInfo(method);
                                    commands.Add(commandInfo.Name, commandInfo);
                                    validCommands.Add(commandInfo.Name, commandInfo);
                                }
                                catch (Exception exception)
                                {
                                    var exceptionLog = new Log(exception.Message, Severity.Error, exception);
                                    OnLog?.Invoke(exceptionLog);
                                    throw;
                                }
                            }
                        }

                        var readOnlyValidCommands = new ReadOnlyDictionary<string, CommandInfo>(commands);
                        var moduleInfo = new ModuleInfo(type, readOnlyValidCommands);

                        validModuleTypes.Add(moduleInfo);
                        totalCommands += commands.Count;
                    }
                }

                _modules = validModuleTypes.AsReadOnly();
                _commands = new ReadOnlyDictionary<string, CommandInfo>(validCommands);

                var successLog = new Log($"Loaded {_modules.Count} modules with {totalCommands} commands in total.", Severity.Info);
                OnLog?.Invoke(successLog);
            });
        }

        public Task HandleCommandAsync(Message message)
        {
            string? text = message.Text;

            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Message text was empty.");
            }

            string[] commandParts = text.Split(ArgsSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            string commandName = commandParts.First().ToLower();
            string[] args = commandParts.Skip(1).ToArray();

            return ExecuteCommandAsync(commandName, args, message);
        }

        protected Task ExecuteCommandAsync(string commandName, string[] args, Message message)
        {
            if (_commands == null || _commands.Count == 0)
            {
                throw new Exception("There are no commands loaded.");
            }

            if (_commands.TryGetValue(commandName, out CommandInfo? command))
            {
                Type commandModuleType = command.CommandHandler.DeclaringType!;
                object commandModuleInstance = ActivatorUtilities.CreateInstance(Services, commandModuleType);
                SetModuleContext(message, commandModuleType, commandModuleInstance);
                return (Task)command.CommandHandler.Invoke(commandModuleInstance, args)!;
            }

            if (ThrowOnUnknownCommand)
            {
                throw new UnknownCommandException("Unknown command.", commandName, this);
            }

            return Task.CompletedTask;
        }

        protected void SetModuleContext(Message message, Type moduleType, object moduleInstance)
        {
            ModuleContext ctx = new ModuleContext(message, this);
            PropertyInfo? prop = moduleType.GetProperty("Context");

            if (prop == null)
            {
                throw new InvalidModuleException($"The module doesn't have property \"Context\".", moduleType);
            }

            prop.SetValue(moduleInstance, ctx);
        }

        protected static CommandInfo MakeCommandInfo(MethodInfo method)
        {
            var commandAttribute = method.GetCustomAttribute<CommandAttribute>();

            if (commandAttribute == null)
            {
                throw new Exception("Method doesn't have Command attribute.");
            }

            return new CommandInfo(commandAttribute.Name, commandAttribute.Description, method);
        }

        protected static bool IsValidModuleType(TypeInfo type)
            => type.IsPublic
            && type.IsClass
            && !type.IsAbstract
            && (type.IsAssignableTo(typeof(CommandsModule)) || type.IsAssignableTo(typeof(ICommandsModule)));

        protected static bool IsValidCommandHandler(MethodInfo method)
            => method.IsPublic
            && !method.IsAbstract
            && method.GetCustomAttribute<CommandAttribute>() != null
            && (method.ReturnType == typeof(Task) || (method.ReturnType == typeof(Task<>)));
    }
}
