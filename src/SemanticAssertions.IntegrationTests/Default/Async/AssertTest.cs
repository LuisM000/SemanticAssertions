using SemanticAssertions.IntegrationTests.Shared;
using SemanticAssertions.Providers;

namespace SemanticAssertions.IntegrationTests.Default.Async;

// ReSharper disable ClassNeverInstantiated.Global
public class AssertShould : AssertTestBase
    // ReSharper restore ClassNeverInstantiated.Global
{
    public AssertShould()
    {
        Configuration.AssertProvider.AddAssertProvider(new DefaultAssertProvider());
    }
}