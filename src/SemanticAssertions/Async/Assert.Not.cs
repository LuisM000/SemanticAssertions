using SemanticAssertions.Abstractions.Diagnostics;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace SemanticAssertions.Async;

public static partial class Assert
{
    public static class Not
    {
#pragma warning disable S3218
        public static async Task AreSimilar(string expected, string actual)
#pragma warning restore S3218
        {
            try
            {
                await Assert.AreSimilar(expected, actual);
            }
            catch (SemanticAssertionsException)
            {
                return;
            }
        
            throw new SemanticAssertionsException($"Strings are similar");
        }
    
#pragma warning disable S3218
        public static async Task AreSimilar(string expected, string actual, double similarityThreshold)
#pragma warning restore S3218
        {
            try
            {
                await Assert.AreSimilar(expected, actual, similarityThreshold);
            }
            catch (SemanticAssertionsException)
            {
                return;
            }
        
            throw new SemanticAssertionsException($"Strings are similar. Max similarity: {similarityThreshold}.");
        }
        
#pragma warning disable S3218
        public static async Task ContainsInformationSubset(string expected, string actual)
#pragma warning restore S3218
        {
            try
            {
                await Assert.ContainsInformationSubset(expected, actual);
            }
            catch (SemanticAssertionsException)
            {
                return;
            }
        
            throw new SemanticAssertionsException($"The string {nameof(actual)} does contain information included in the string {nameof(expected)}");
        }

#pragma warning disable S3218
        public static async Task AreInSameLanguage(string expected, string actual)
#pragma warning restore S3218
        {
            try
            {
                await Assert.AreInSameLanguage(expected, actual);
            }
            catch (SemanticAssertionsException)
            {
                return;
            }
        
            throw new SemanticAssertionsException($"The {nameof(actual)} value is in same language as {nameof(expected)} value.");
        }
    }
}