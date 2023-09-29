namespace SemanticAssertions.Internals.Abstractions;

internal interface IAssertHandler
{
    Task<string> CalculateSimilarityAsync(string expected, string actual);

    Task<string> AreInSameLanguage(string expected, string actual);
}