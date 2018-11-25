using System;
using AlleyCat.Common;
using EnsureThat;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace AlleyCat.Logging
{
    public interface ILoggable
    {
        ILogger Logger { get; }
    }

    public static class LoggableExtensions
    {
        public static string GetLogCategory(this ILoggable loggable)
        {
            Ensure.That(loggable, nameof(loggable)).IsNotNull();

            return loggable is IIdentifiable identifiable
                ? string.Join(":", loggable.GetType().FullName, identifiable.Key)
                : loggable.GetType().FullName;
        }

        public static void LogTrace(this ILoggable loggable, string message, params object[] args) =>
            Log(loggable, LogLevel.Trace, message, args);

        public static void LogDebug(this ILoggable loggable, string message, params object[] args) =>
            Log(loggable, LogLevel.Debug, message, args);

        public static void LogInfo(this ILoggable loggable, string message, params object[] args) =>
            Log(loggable, LogLevel.Information, message, args);

        public static void LogWarn(this ILoggable loggable, string message, params object[] args) =>
            Log(loggable, LogLevel.Warning, message, args);

        public static void LogWarn(
            this ILoggable loggable,
            Exception exception,
            string message,
            params object[] args) => Log(loggable, LogLevel.Warning, message, args, exception);

        public static void LogError(this ILoggable loggable, string message, params object[] args) =>
            Log(loggable, LogLevel.Error, message, args);

        public static void LogError(
            this ILoggable loggable,
            Exception exception,
            string message,
            params object[] args) => Log(loggable, LogLevel.Error, message, args, exception);

        public static void LogFatal(this ILoggable loggable, string message, params object[] args) =>
            Log(loggable, LogLevel.Critical, message, args);

        public static void LogFatal(
            this ILoggable loggable,
            Exception exception,
            string message,
            params object[] args) => Log(loggable, LogLevel.Critical, message, args, exception);

        private static void Log(
            ILoggable loggable,
            LogLevel level,
            string message,
            object[] args,
            [CanBeNull] Exception exception = null)
        {
            Ensure.That(loggable, nameof(loggable)).IsNotNull();

            var logger = loggable.Logger;

            if (!logger.IsEnabled(level)) return;

            var evalArgs = args.Map(a => a is Func<object> fun ? fun.Invoke() : a);

            switch (level)
            {
                case LogLevel.Trace:
                    logger.LogTrace(message, evalArgs);
                    break;
                case LogLevel.Debug:
                    logger.LogDebug(message, evalArgs);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(message, evalArgs);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(exception, message, evalArgs);
                    break;
                case LogLevel.Error:
                    logger.LogError(exception, message, evalArgs);
                    break;
                case LogLevel.Critical:
                    logger.LogCritical(exception, message, evalArgs);
                    break;
            }
        }
    }
}