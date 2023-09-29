using SemanticAssertions.Internals;

namespace SemanticAssertions.Async;

public static class Assert
{
    private static readonly IAssertHandler assertHandler;
    
    static Assert()
    {
        // ToDo: review this...
        // Should be with DI???
        assertHandler = new AssertProvider().GetAssertHandler();
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
        
        //ToDo: improve exceptions
        throw new Exception($"Strings are not similar. Expected: '{expected}', Actual: '{actual}', Current similarity: {similarity}");
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
        
        //ToDo: improve exceptions
        throw new Exception($"The {nameof(actual)} is not in same language as {nameof(expected)}");
    }

    private static bool TryParseDouble(string text, out double result)
    {
        return double.TryParse(text.Replace(",", "."), out result);
    }
}