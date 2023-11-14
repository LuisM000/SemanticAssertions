// ReSharper disable MemberHidesStaticFromOuterClass
namespace SemanticAssertions.Internals.SemanticKernel.Plugins.SimilarityPlugin;

internal static class SimilarityPluginInfo
{
    public const string Name = "SimilarityPlugin";
    
    public static class AreSimilar
    {
#pragma warning disable S3218
        public const string Name = nameof(AreSimilar);
#pragma warning restore S3218
    }
    
    public static class CalculateSimilarity
    {
#pragma warning disable S3218
        public const string Name = nameof(CalculateSimilarity);
#pragma warning restore S3218
    }
}