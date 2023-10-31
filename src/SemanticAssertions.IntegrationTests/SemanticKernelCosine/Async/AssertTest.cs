using SemanticAssertions.IntegrationTests.Shared;
using SemanticAssertions.Providers;

namespace SemanticAssertions.IntegrationTests.SemanticKernelCosine.Async;

// ReSharper disable ClassNeverInstantiated.Global
public class AssertShould : AssertTestBase
    // ReSharper restore ClassNeverInstantiated.Global
{    

    public AssertShould()
    {
        Configuration.Current.AddAssertProvider(new SKCosineAssertProvider());
    }
}