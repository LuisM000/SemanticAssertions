namespace SemanticAssertions.Abstractions;

public interface IAssertHandler
{
    Task<string> AreSimilar(string expected, string actual);
    
    Task<string> CalculateSimilarityAsync(string expected, string actual);

    Task<string> AreInSameLanguage(string expected, string actual);
}