// ReSharper disable MemberHidesStaticFromOuterClass
namespace SemanticAssertions.Internals.SemanticKernel.Plugins.SimilarityPlugin;

internal static class SimilarityPluginInfo
{
    public const string Name = "SimilarityPlugin";
    
    public static class AreSimilar
    {
        public const string Name = nameof(AreSimilar);
        
        public static class Parameters
        {
            public static readonly string Actual = nameof(Actual).ToLowerInvariant();
       
            public static readonly string Expected = nameof(Expected).ToLowerInvariant();
        }
    }
}