using Xunit;

namespace TestProject;

[CollectionDefinition(nameof(AllTestsCollectionFixture))]
public class AllTestsCollectionFixture : ICollectionFixture<AutoMapperFixture>
{

}