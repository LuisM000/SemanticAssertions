using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Async;

public static class Assert
{
    private static readonly IAssertHandler assertHandler;
    
    static Assert()
    {
        assertHandler = Configuration.AssertProvider.Provider.GetAssertHandler();
    }
    
    public static async Task AreSimilar(string expected, string actual, double similarity)
    {
        if (string.IsNullOrEmpty(expected) && string.IsNullOrEmpty(actual))
        {
            return;
        }
        
        var result = await assertHandler.CalculateSimilarityAsync(expected, actual);
        
        if (TryParseDouble(result, out var similarityResult)
            && similarityResult >= similarity)
        {
           return;
        }
        
        throw new SemanticAssertionsException($"Strings are not similar. Expected similarity: {similarity}. Actual similarity: {result}");
    }

    public static async Task AreInSameLanguage(string expected, string actual)
    {
        if (string.IsNullOrEmpty(expected) && string.IsNullOrEmpty(actual))
        {
            return;
        }
        
        var result = await assertHandler.AreInSameLanguage(expected, actual);
        
        if (bool.TryParse(result, out var languageResult) 
            && languageResult)
        {
            return;
        }
        
        throw new SemanticAssertionsException($"The {nameof(actual)} value is not in same language as {nameof(expected)} value");
    }

    private static bool TryParseDouble(string text, out double result)
    {
        return double.TryParse(text.Replace(",", "."), out result);
    }
}