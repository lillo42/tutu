using Tutu.Exceptions;

namespace Tutu.Windows.Exceptions;

internal class FailToWaitPollException : TutuException
{
    public FailToWaitPollException(string? message) : base(message)
    {
    }
}
