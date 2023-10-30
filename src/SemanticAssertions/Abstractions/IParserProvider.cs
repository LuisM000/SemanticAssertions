namespace SemanticAssertions.Abstractions;

public interface IParserProvider
{
    IParserHandler GetParserHandler();
}