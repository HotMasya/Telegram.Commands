namespace Masya.Telegram.Commands
{
    public sealed class Log
    {
        public string Message { get; }
        public DateTime TimeStamp { get; }
        public Severity Severity { get; }
        public Exception? Exception { get; }

        public Log(string message, DateTime timeStamp, Severity severity)
        {
            Message = message;
            TimeStamp = timeStamp;
            Severity = severity;
        }

        public Log(string message, Severity severity) 
            : this(message, DateTime.Now, severity) { }

        public Log(string message, DateTime timeStamp, Severity severity, Exception exception) 
            : this(message, timeStamp, severity)
        {
            Exception = exception;
        }

        public Log(string message, Severity severity, Exception exception) 
            : this(message, DateTime.Now, severity, exception) { }
    }
}