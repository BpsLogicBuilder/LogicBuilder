using Xunit;

namespace Contoso.Test.Flow.Test
{
    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code. It is only used to anchor the shared fixture.
    }
}
