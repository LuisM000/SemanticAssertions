using SemanticAssertions.Abstractions;
using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Providers;

internal class DefaultAssertProvider : IAssertProvider
{
    public IAssertHandler GetAssertHandler()
    {
        return new SKAssertHandler();
    }
}