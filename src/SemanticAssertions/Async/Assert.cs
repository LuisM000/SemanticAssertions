using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Async;

public static class Assert
{
    private static IAssertHandler AssertHandler =>
        Configuration.AssertProvider.Provider.GetAssertHandler();

    private static IParserHandler ParserProvider =>
        Configuration.ParserProvider.Provider.GetParserHandler();
      
    public static async Task AreSimilar(string expected, string actual)
    {
        if (string.IsNullOrEmpty(expected) && string.IsNullOrEmpty(actual))
        {
            return;
        }
        
        var result = await AssertHandler.AreSimilar(expected, actual);

        var areSimilar = await ParserProvider.ParseBoolAsync(result);
        
        if (areSimilar)
        {
            return;
        }
        
        throw new SemanticAssertionsException($"Strings are not similar");
    }
    
    public static async Task AreSimilar(string expected, string actual, double similarityThreshold)
    {
        if (string.IsNullOrEmpty(expected) && string.IsNullOrEmpty(actual))
        {
            return;
        }
        
        var result = await AssertHandler.CalculateSimilarityAsync(expected, actual);

        var similarity = await ParserProvider.ParseDoubleAsync(result);
        
        if (similarity >= similarityThreshold)
        {
            return;
        }
        
        throw new SemanticAssertionsException($"Strings are not similar. Expected similarity: {similarityThreshold}. Actual similarity: {result}");
    }

    public static async Task AreInSameLanguage(string expected, string actual)
    {
        if (string.IsNullOrEmpty(expected) && string.IsNullOrEmpty(actual))
        {
            return;
        }
        
        var result = await AssertHandler.AreInSameLanguage(expected, actual);

        var areInSameLanguage = await ParserProvider.ParseBoolAsync(result);
        
        if (areInSameLanguage)
        {
            return;
        }
        
        throw new SemanticAssertionsException($"The {nameof(actual)} value is not in same language as {nameof(expected)} value");
    }
}