// ReSharper disable MemberHidesStaticFromOuterClass
namespace SemanticAssertions.Internals.SemanticKernel.Plugins.LanguagePlugin;

internal static class LanguagePluginInfo
{
    public const string Name = "LanguagePlugin";
    
    public static class AreInSameLanguage
    {
#pragma warning disable S3218
        public const string Name = nameof(AreInSameLanguage);
#pragma warning restore S3218
    }
}