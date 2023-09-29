namespace SemanticAssertions.Internals.Abstractions;

internal interface IAssertProvider
{
    IAssertHandler GetAssertHandler();
}