using SemanticAssertions.Abstractions;
using SemanticAssertions.Internals;

namespace SemanticAssertions.Providers;

public class DefaultParserProvider : IParserProvider
{
    public IParserHandler GetParserHandler()
    {
        return new ParserManagerHandler(new SimpleParserHandler());
    }
}