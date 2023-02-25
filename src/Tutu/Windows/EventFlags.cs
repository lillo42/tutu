namespace Tutu.Windows;

internal static partial class Windows
{
    /// <summary>
    /// The type of mouse event.
    /// If this value is zero, it indicates a mouse button being pressed or released.
    /// Otherwise, this member is one of the following values.
    ///
    /// [Ms Docs](https://docs.microsoft.com/en-us/windows/console/mouse-event-record-str#members)
    /// </summary>
    public enum EventFlags
    {
        PressOrRelease = 0x0000,

        /// <summary>
        /// The second click (button press) of a double-click occurred. The first click is returned as a regular button-press event.
        /// </summary>
        DoubleClick = 0x0002,

        /// <summary>
        /// The horizontal mouse wheel was moved.
        /// </summary>
        MouseHwheeled = 0x0008,

        /// <summary>
        /// If the high word of the dwButtonState member contains a positive value, the wheel was rotated to the right. Otherwise, the wheel was rotated to the left.
        /// </summary>
        MouseMoved = 0x0001,

        /// <summary>
        /// A change in mouse position occurred.
        /// The vertical mouse wheel was moved, if the high word of the dwButtonState member contains a positive value, the wheel was rotated forward, away from the user.
        /// Otherwise, the wheel was rotated backward, toward the user.
        /// </summary>
        MouseWheeled = 0x0004,

        /// <summary>
        /// This button state is not recognized.
        /// </summary>
        Unknown = 0x0021,
    }
}