using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Internals.Abstractions;

internal class AssertProvider : IAssertProvider
{
    public IAssertHandler GetAssertHandler()
    {
        return new SKAssertHandler();
    }
}