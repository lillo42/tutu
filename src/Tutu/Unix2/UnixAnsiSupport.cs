namespace Tutu.Unix2;

/// <summary>
/// Unix implementation of <see cref="IAnsiSupport"/>.
/// </summary>
public class UnixAnsiSupport : IAnsiSupport
{
    /// <inheritdoc cref="IAnsiSupport.IsAnsiSupported" />
    public bool IsAnsiSupported => true;
}