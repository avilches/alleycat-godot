﻿using System;
using AlleyCat.UI.Console;
using EnsureThat;
using Godot;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace AlleyCat.Logging
{
    public class ConsoleLogger : ILogger
    {
        [NotNull]
        public string Name { get; }

        [NotNull]
        public IConsole Console { get; }

        public ConsoleLogger([NotNull] string name, [NotNull] IConsole console)
        {
            Ensure.String.IsNotNullOrWhiteSpace(name, nameof(name));
            Ensure.Any.IsNotNull(console, nameof(console));

            Name = name;
            Console = console;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            [CanBeNull] Exception exception,
            [NotNull] Func<TState, Exception, string> formatter)
        {
            Ensure.Any.IsNotNull(formatter, nameof(formatter));

            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                var prefix = GetLevelPrefix(logLevel);

                Color color;

                switch (logLevel)
                {
                    case LogLevel.Warning:
                        color = Console.WarningColor;
                        break;
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        color = Console.ErrorColor;
                        break;
                    default:
                        color = Console.TextColor;
                        break;
                }

                var highlight = new TextStyle(Console.HighlightColor);

                // ReSharper disable once AssignNullToNotNullAttribute
                Console
                    .Write("[", highlight)
                    .Write(prefix, new TextStyle(color))
                    .Write("]", highlight)
                    .Write("[", highlight)
                    .Write(Name)
                    .Write("] ", highlight)
                    .WriteLine(message);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        protected string GetLevelPrefix(LogLevel level)
        {
            var prefix = level.ToString();

            return prefix.Length < 6 ? prefix : prefix.Left(4);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
