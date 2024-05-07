namespace Tutu.Unix;

/// <summary>
/// Unix implementation of <see cref="IAnsiSupport"/>.
/// </summary>
public sealed class UnixAnsiSupport : IAnsiSupport
{
    /// <inheritdoc cref="IAnsiSupport.IsAnsiSupported" />
    public bool IsAnsiSupported => true;
}
