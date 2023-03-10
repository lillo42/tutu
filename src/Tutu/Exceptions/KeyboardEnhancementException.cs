namespace Tutu.Exceptions;

/// <summary>
/// The exception that is thrown when could not check if OS has support to keyboard enhancement.
/// </summary>
public class KeyboardEnhancementException : TutuException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardEnhancementException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public KeyboardEnhancementException(string? message)
        : base(message)
    {
    }
}
