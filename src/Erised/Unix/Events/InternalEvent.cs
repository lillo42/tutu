using Erised.Events;

namespace Erised;

internal static partial class Unix
{
    public interface IInternalEvent { }
    
    public record PublicEvent(IEvent Event) : IInternalEvent;

    public record CursorPosition(ushort Column, ushort Row) : IInternalEvent;

    public record KeyboardEnhancementFlagsEvent(KeyboardEnhancementFlags Flags);

    public record PrimaryDeviceAttributes : IInternalEvent;
}