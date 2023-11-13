using SemanticAssertions.IntegrationTests.Asserts.Shared;
using SemanticAssertions.Providers;
using Xunit.Abstractions;

namespace SemanticAssertions.IntegrationTests.Asserts.SemanticKernel.Async;

// ReSharper disable ClassNeverInstantiated.Global
public class AssertShould : AssertTestBase
// ReSharper restore ClassNeverInstantiated.Global
{
    public AssertShould(ITestOutputHelper output) : base(output)
    {
        Configuration.Current.AddAssertProvider(new SKAssertProvider());
    }
}