namespace SemanticAssertions.Abstractions;

public interface IAssertProvider
{
    IAssertHandler GetAssertHandler();
}