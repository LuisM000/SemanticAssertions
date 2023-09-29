using SemanticAssertions.IntegrationTests.Shared;
using SemanticAssertions.Providers;

namespace SemanticAssertions.IntegrationTests.SemanticKernel.Async;

// ReSharper disable ClassNeverInstantiated.Global
public class AssertShould : AssertTestBase
    // ReSharper restore ClassNeverInstantiated.Global
{
    public AssertShould()
    {
        Configuration.AssertProvider.AddAssertProvider(new SKAssertProvider());
    }
}