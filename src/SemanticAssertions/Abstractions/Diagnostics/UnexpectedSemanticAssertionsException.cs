namespace SemanticAssertions.Abstractions.Diagnostics;

public class UnexpectedSemanticAssertionsException : Exception
{
    public UnexpectedSemanticAssertionsException(string message) : base(message)
    {
    }
    
    public UnexpectedSemanticAssertionsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}