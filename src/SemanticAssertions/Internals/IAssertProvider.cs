namespace SemanticAssertions.Internals;

internal interface IAssertProvider
{
    IAssertHandler GetAssertHandler();
}