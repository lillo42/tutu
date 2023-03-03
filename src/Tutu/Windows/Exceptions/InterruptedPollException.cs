using Tutu.Exceptions;

namespace Tutu.Windows.Exceptions;

internal class InterruptedPollException : TutuException
{
    public InterruptedPollException(string? message) : base(message)
    {

    }
}
