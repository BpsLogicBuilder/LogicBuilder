﻿using Microsoft.Extensions.Logging;
using System;
using System.Text;
using Xunit.Abstractions;

namespace Contoso.Test.Flow.Test
{
    internal class XUnitLogger(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider, string categoryName) : ILogger
    {
        private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;
        private readonly string _categoryName = categoryName;
        private readonly LoggerExternalScopeProvider _scopeProvider = scopeProvider;

        public static ILogger CreateLogger(ITestOutputHelper testOutputHelper) => new XUnitLogger(testOutputHelper, new LoggerExternalScopeProvider(), "");
        public static ILogger<T> CreateLogger<T>(ITestOutputHelper testOutputHelper)
        {
            return new XUnitLogger<T>(testOutputHelper, new LoggerExternalScopeProvider());
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => _scopeProvider.Push(state);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            var sb = new StringBuilder();
            sb.Append(GetLogLevelString(logLevel))
              .Append(" [").Append(_categoryName).Append("] ")
              .Append(formatter(state, exception!));

            if (exception != null)
            {
                sb.Append('\n').Append(exception);
            }

            // Append scopes
            _scopeProvider.ForEachScope((scope, state) =>
            {
                state.Append("\n => ");
                state.Append(scope);
            }, sb);

            _testOutputHelper.WriteLine(sb.ToString());
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "trce",
                LogLevel.Debug => "dbug",
                LogLevel.Information => "info",
                LogLevel.Warning => "warn",
                LogLevel.Error => "fail",
                LogLevel.Critical => "crit",
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
            };
        }
    }

    internal sealed class XUnitLogger<T>(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider) : XUnitLogger(testOutputHelper, scopeProvider, typeof(T).FullName!), ILogger<T>
    {
    }
}
