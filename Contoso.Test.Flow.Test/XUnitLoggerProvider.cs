﻿using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Contoso.Test.Flow.Test
{
    internal sealed class XUnitLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly LoggerExternalScopeProvider _scopeProvider = new();

        public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(_testOutputHelper, _scopeProvider, categoryName);
        }

        public void Dispose()
        {
        }
    }
}
