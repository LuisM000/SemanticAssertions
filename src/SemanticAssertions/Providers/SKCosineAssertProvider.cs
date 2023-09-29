using SemanticAssertions.Abstractions;
using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Providers;

// ReSharper disable InconsistentNaming
public class SKCosineAssertProvider: IAssertProvider
    // ReSharper restore InconsistentNaming
{
    public IAssertHandler GetAssertHandler()
    {
        return new SKCosineAssertHandler();
    }
}