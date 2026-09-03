using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace Contoso.Test.Flow.Test
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest")//2025-latest
                                .Build();

        private readonly ConcurrentBag<string> _createdDatabases = [];

        public string GetConnectionString(string initialCatalog)
        {
            // Track the db name so we can clean it up later if desired
            _createdDatabases.Add(initialCatalog);

            return new SqlConnectionStringBuilder(_msSqlContainer.GetConnectionString())
            {
                InitialCatalog = initialCatalog,
                TrustServerCertificate = true // Prevents SSL negotiation errors in CI
            }.ToString();
        }

        async ValueTask IAsyncLifetime.InitializeAsync()
        {
            // Give CI up to 5 minutes to pull and start the container under heavy load
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            await _msSqlContainer.StartAsync(cts.Token);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_msSqlContainer != null)
                await _msSqlContainer.DisposeAsync();

            GC.SuppressFinalize(this);
        }
    }
}
