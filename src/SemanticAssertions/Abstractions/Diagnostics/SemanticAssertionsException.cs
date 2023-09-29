namespace SemanticAssertions.Abstractions.Diagnostics;

public class SemanticAssertionsException : Exception
{
    public SemanticAssertionsException(string message) : base(message)
    {
    }
    
    public SemanticAssertionsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}