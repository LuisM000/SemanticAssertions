using SemanticAssertions.Abstractions;
using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Providers;

public class DefaultAssertProvider : IAssertProvider
{
    public IAssertHandler GetAssertHandler()
    {
        return new SKAssertHandler();
    }
}