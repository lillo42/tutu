namespace Tutu.Exceptions;

internal class CouldNotParseEventException : TutuException
{
    public CouldNotParseEventException(string? message) : base(message)
    {
    }
}
