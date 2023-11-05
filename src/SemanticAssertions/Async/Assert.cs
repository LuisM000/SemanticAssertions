using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Async;

public static class Assert
{
    private static IAssertHandler AssertHandler =>
        Configuration.Current.AssertProvider.GetAssertHandler();

    private static IParserHandler ParserProvider =>
        Configuration.Current.ParserProvider.GetParserHandler();
      
    public static async Task AreSimilar(string expected, string actual)
    {
        if (string.IsNullOrEmpty(expected) && string.IsNullOrEmpty(actual))
        {
            return;
        }
        
        var result = await AssertHandler.AreSimilar(expected, actual).ConfigureAwait(false);

        var areSimilar = await ParserProvider.ParseBoolAsync(result).ConfigureAwait(false);
        
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
        
        var result = await AssertHandler.CalculateSimilarityAsync(expected, actual).ConfigureAwait(false);

        var similarity = await ParserProvider.ParseDoubleAsync(result).ConfigureAwait(false);
        
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
        
        var result = await AssertHandler.AreInSameLanguage(expected, actual).ConfigureAwait(false);

        var areInSameLanguage = await ParserProvider.ParseBoolAsync(result).ConfigureAwait(false);
        
        if (areInSameLanguage)
        {
            return;
        }
        
        throw new SemanticAssertionsException($"The {nameof(actual)} value is not in same language as {nameof(expected)} value");
    }
}