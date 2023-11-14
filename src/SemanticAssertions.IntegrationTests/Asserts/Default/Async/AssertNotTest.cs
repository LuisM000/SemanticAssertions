using SemanticAssertions.IntegrationTests.Asserts.Shared;
using SemanticAssertions.Providers;
using Xunit.Abstractions;

namespace SemanticAssertions.IntegrationTests.Asserts.Default.Async;

// ReSharper disable ClassNeverInstantiated.Global
public class AssertNotShould : AssertNotTestBase
// ReSharper restore ClassNeverInstantiated.Global
{
    public AssertNotShould(ITestOutputHelper output) : base(output)
    {
        Configuration.Current.AddAssertProvider(new DefaultAssertProvider());
    }
}