namespace Tutu.Events;

/// <summary>
/// Interface for filtering an `InternalEvent`.
/// </summary>
public interface IFilter
{
    /// <summary>
    /// Returns whether the given event fulfills the filter.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    bool Eval(IInternalEvent @event);
}

/// <summary>
/// Public <see cref="IEvent"/> filter.
/// </summary>
public record PublicEventFilter : IFilter
{
    /// <summary>
    /// The default filter.
    /// </summary>
    public static PublicEventFilter Default { get; } = new();

    /// <inheritdoc />
    public bool Eval(IInternalEvent @event) => @event is PublicEvent;
}

/// <summary>
/// The cursor position filter.
/// </summary>
public record CursorPositionFilter : IFilter
{
    /// <summary>
    /// The default filter.
    /// </summary>
    public static CursorPositionFilter Default { get; } = new();

    /// <inheritdoc />
    public bool Eval(IInternalEvent @event) => @event is CursorPositionInternalEvent;
}

/// <summary>
/// The keyboard enhancement flags filter.
/// </summary>
public record KeyboardEnhancementFlagsFilter : IFilter
{
    /// <summary>
    /// The default filter.
    /// </summary>
    public static KeyboardEnhancementFlagsFilter Default { get; } = new();

    /// <inheritdoc />
    public bool Eval(IInternalEvent @event) => @event is KeyboardEnhancementFlagsInternalEvent;
}

/// <summary>
/// The primary device attributes filter.
/// </summary>
public record PrimaryDeviceAttributesFilter : IFilter
{
    /// <summary>
    /// The default filter.
    /// </summary>
    public static PrimaryDeviceAttributesFilter Default { get; } = new();
    
    /// <inheritdoc />
    public bool Eval(IInternalEvent @event) => @event is PrimaryDeviceAttributesInternalEvent;
}