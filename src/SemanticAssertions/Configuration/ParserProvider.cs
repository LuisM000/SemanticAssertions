using SemanticAssertions.Abstractions;
using SemanticAssertions.Providers;

namespace SemanticAssertions.Configuration;

public static class ParserProvider
{
    internal static IParserProvider Provider { get; private set; } = new DefaultParserProvider();

    public static void AddParserProvider(IParserProvider provider)
    {
        Provider = provider;
    }
}