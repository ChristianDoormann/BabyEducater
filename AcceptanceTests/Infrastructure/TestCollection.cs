using Xunit;

namespace AcceptanceTests.Infrastructure
{
    [CollectionDefinition(nameof(TestCollection))]
    public class TestCollection : ICollectionFixture<TestFixtureData>
    {
        
    }
}