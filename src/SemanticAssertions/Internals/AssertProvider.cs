using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Internals;

internal class AssertProvider : IAssertProvider
{
    public IAssertHandler GetAssertHandler()
    {
        return new SKAssertHandler();
    }
}