using SemanticAssertions.Abstractions;
using SemanticAssertions.Internals;
using SemanticAssertions.Internals.SemanticKernel;

namespace SemanticAssertions.Providers;

public class DefaultParserProvider : IParserProvider
{
    public IParserHandler GetParserHandler()
    {
        return new ParserManagerHandler(new SimpleParserHandler(), new SKFunctionCallingParserHandler());
    }
}