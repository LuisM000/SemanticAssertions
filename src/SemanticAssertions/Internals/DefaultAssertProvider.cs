using SemanticAssertions.Abstractions;
using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Internals;

internal class DefaultAssertProvider : IAssertProvider
{
    public IAssertHandler GetAssertHandler()
    {
        return new SKAssertHandler();
    }
}