using Xunit;

namespace TestProject.Base;

[CollectionDefinition(nameof(AllTestsCollectionFixture))]
public class AllTestsCollectionFixture : ICollectionFixture<AutoMapperFixture> { }