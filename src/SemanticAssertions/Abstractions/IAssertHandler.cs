namespace SemanticAssertions.Abstractions;

public interface IAssertHandler
{
    Task<string> CalculateSimilarityAsync(string expected, string actual);

    Task<string> AreInSameLanguage(string expected, string actual);
}