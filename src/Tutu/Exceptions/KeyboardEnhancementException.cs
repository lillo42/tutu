namespace Tutu.Exceptions;

/// <summary>
/// The exception that is thrown when could not check if OS has support to keyboard enhancement.
/// </summary>
public class KeyboardEnhancementException : TutuException
{
    public KeyboardEnhancementException(string? message) 
        : base(message)
    {
    }
}