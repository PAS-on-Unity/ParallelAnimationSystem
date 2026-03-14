using System;
using Microsoft.Extensions.Logging;

namespace ParallelAnimationSystem.Unity.Implementation;

public class UnityLoggerProvider : ILoggerProvider
{
    private class UnityLogger(string categoryName) : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    UnityEngine.Debug.Log($"[{categoryName}] {message}");
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning($"[{categoryName}] {message}");
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    UnityEngine.Debug.LogError($"[{categoryName}] {message}");
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
            => true;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            => throw new NotImplementedException();
    }
    
    public ILogger CreateLogger(string categoryName)
        => new UnityLogger(categoryName);
    
    public void Dispose()
    {
    }
}