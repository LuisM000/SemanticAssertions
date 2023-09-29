namespace SemanticAssertions.Internals.SemanticKernel.Plugins;

internal static class PluginsInfo
{
    public static readonly string Directory =
        Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Internals/SemanticKernel/Plugins");
    
    public static class Parameters
    {
        public static readonly string Actual = nameof(Actual).ToLowerInvariant();
       
        public static readonly string Expected = nameof(Expected).ToLowerInvariant();
    }
}