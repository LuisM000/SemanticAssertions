using SemanticAssertions.Abstractions;
using SemanticAssertions.Providers;

namespace SemanticAssertions.Configuration;

public static class AssertProvider
{
    internal static IAssertProvider Provider { get; private set; } = new DefaultAssertProvider();

    public static void AddAssertProvider(IAssertProvider provider)
    {
        Provider = provider;
    }
}