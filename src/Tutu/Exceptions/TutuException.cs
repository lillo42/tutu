using System.Runtime.Serialization;

namespace Tutu.Exceptions;

/// <summary>
/// The base exception for all Tutu exceptions.
/// </summary>
public abstract class TutuException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TutuException"/> class.
    /// </summary>
    protected TutuException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TutuException"/> class.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/>.</param>
    /// <param name="context">The <see cref="StreamContent"/>.</param>
    protected TutuException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TutuException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    protected TutuException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TutuException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner <see cref="Exception"/>.</param>
    protected TutuException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
