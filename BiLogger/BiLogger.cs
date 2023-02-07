using Microsoft.Extensions.Logging;
using System.Runtime.ConstrainedExecution;

namespace BiLogger
{
    public class BiLogger : ILogger, IDisposable
    {
        private readonly ILogger _logger;
        private readonly StreamWriter _writer;

        /// <summary>
        /// To Write to a file:
        /// StreamWriter sw = new StreamWriter(
        ///      new FileStream(fileName, FileMode.OpenOrCreate), 
        ///      Encoding.UTF8
        /// );
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="writer"></param>
        public BiLogger(ILogger logger, StreamWriter writer) 
        {
            _logger = logger;
            _writer = writer;
            _writer.AutoFlush = true;
        }

        ~BiLogger()
        {
            if (_writer != null)
            {
                _writer.Close();
            }
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Close();
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
            _writer.Write($"{exception?.Message}");
        }        
    }
}