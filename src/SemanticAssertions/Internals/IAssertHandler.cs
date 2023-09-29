namespace SemanticAssertions.Internals;

internal interface IAssertHandler
{
    Task<string> CalculateSimilarityAsync(string expected, string actual);

    Task<string> AreInSameLanguage(string expected, string actual);
}