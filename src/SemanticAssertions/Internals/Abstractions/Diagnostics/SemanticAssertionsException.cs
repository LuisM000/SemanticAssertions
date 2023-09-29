namespace SemanticAssertions.Internals.Abstractions.Diagnostics;

public class SemanticAssertionsException : Exception
{
    public SemanticAssertionsException(string message) : base(message)
    {
    }
}