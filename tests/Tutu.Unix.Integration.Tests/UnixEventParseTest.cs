using FluentAssertions;
using Tutu.Events;
using static Tutu.Events.Event;
using static Tutu.Events.InternalEvent;

namespace Tutu.Unix.Integration.Tests;

public class UnixEventParseTest
{
    private readonly UnixEventParse _parse;

    public UnixEventParseTest()
    {
        _parse = new();
    }

    public static IEnumerable<object[]> ParseTest =>
        new List<object[]>
        {
            // Parse Event
            new object[] { "\x1B"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Esc, KeyModifiers.None))) },
            new object[] { "\x1BOD"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.None))) },
            new object[] { "\x1BOC"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Right, KeyModifiers.None))) },
            new object[] { "\x1BOA"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Up, KeyModifiers.None))) },
            new object[] { "\x1BOB"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None))) },
            new object[] { "\x1BOH"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Home, KeyModifiers.None))) },
            new object[] { "\x1BOF"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.End, KeyModifiers.None))) },
            new object[] { "\x1BOP"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(1), KeyModifiers.None))) },
            new object[] { "\x1BOQ"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(2), KeyModifiers.None))) },
            new object[] { "\x1BOR"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(3), KeyModifiers.None))) },
            new object[] { "\x1BOS"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(4), KeyModifiers.None))) },
            
            new object[] { "\x1B[[A"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(1), KeyModifiers.None))) },
            new object[] { "\x1B[[B"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(2), KeyModifiers.None))) },
            new object[] { "\x1B[[C"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(3), KeyModifiers.None))) },
            new object[] { "\x1B[[D"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(4), KeyModifiers.None))) },
            new object[] { "\x1B[[E"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(5), KeyModifiers.None))) },
            
            // yield return new object[] { "\n"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Enter, KeyModifiers.None))) };
            new object[] { "\r"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Enter, KeyModifiers.None))) },
            new object[] { "\t"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Tab, KeyModifiers.None))) },
            new object[] { "\x7F"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Backspace, KeyModifiers.None))) },
            new object[] { "\0"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char(' '), KeyModifiers.Control))) },
            
            // Parse CSI
            new object[] { "\x1B[D"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.None))) },
            new object[] { "\x1B[C"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Right, KeyModifiers.None))) },
            new object[] { "\x1B[A"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Up, KeyModifiers.None))) },
            new object[] { "\x1B[B"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None))) },
            new object[] { "\x1B[H"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Home, KeyModifiers.None))) },
            new object[] { "\x1B[F"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.End, KeyModifiers.None))) },
            new object[] { "\x1B[Z"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Backspace, KeyModifiers.Shift, KeyEventKind.Press))) },
            new object[] { "\x1B[I"u8.ToArray(), Event(FocusGained) },
            new object[] { "\x1B[O"u8.ToArray(), Event(FocusLost) },
            new object[] { "\x1B[P"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(1), KeyModifiers.None))) },
            new object[] { "\x1B[Q"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(2), KeyModifiers.None))) },
            new object[] { "\x1B[S"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(4), KeyModifiers.None))) },
            
            
            // Parse CSI Modifier Key Code
            new object[] { "\x1B[2D"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.Shift))) },
            
            // Parse CSI Special Key Code
            new object[] { "\x1B[3~"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Delete, KeyModifiers.None))) },
            new object[] { "\x1B[3;2~"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Delete, KeyModifiers.Shift))) },
            
            // Parse CSI Bracketed Paste
            new object[] { "\x1B[200~o\x1B[2D\x1B[201~"u8.ToArray(), Event(Pasted("o\x1B[2D")) },

            new object[] { "\x1B\x1B"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Esc, KeyModifiers.None, KeyEventKind.Press))) },
            new object[] { new[] { (byte)'\x1B', (byte)'c' }, Event(Key(new KeyEvent(KeyCode.Char('c'), KeyModifiers.Alt))) },
            new object[] { new[] { (byte)'\x1B', (byte)'H' }, Event(Key(new KeyEvent(KeyCode.Char('H'), KeyModifiers.Alt | KeyModifiers.Shift))) },
            new object[] { new[] { (byte)'\x1B', (byte)'\x14', (byte)'t' }, Event(Key(new KeyEvent(KeyCode.Char('t'), KeyModifiers.Alt | KeyModifiers.Control))) },
            
            // Parse CSI RXVT Mouse
            new object[] { "\x1B[32;30;40;M"u8.ToArray(), Event(Mouse(new MouseEvent(MouseEventKind.Down(MouseButton.Left), 29, 39, KeyModifiers.None))) },
            
            // Parse CSI Normal Mouse
            new object[] { "\x1B[M0\x60\x70"u8.ToArray(), Event(Mouse(new MouseEvent(MouseEventKind.Down(MouseButton.Left), 63, 79, KeyModifiers.Control))) },
            
            // Parse CSI SGR Mouse
            new object[] { "\x1B[<0;20;10;M"u8.ToArray(), Event(Mouse(new MouseEvent(MouseEventKind.Down(MouseButton.Left), 19, 9, KeyModifiers.None))) },
            new object[] { "\x1B[<0;20;10M"u8.ToArray(), Event(Mouse(new MouseEvent(MouseEventKind.Down(MouseButton.Left), 19, 9, KeyModifiers.None))) },
            new object[] { "\x1B[<0;20;10;m"u8.ToArray(), Event(Mouse(new MouseEvent(MouseEventKind.Up(MouseButton.Left), 19, 9, KeyModifiers.None))) },
            new object[] { "\x1B[<0;20;10m"u8.ToArray(), Event(Mouse(new MouseEvent(MouseEventKind.Up(MouseButton.Left), 19, 9, KeyModifiers.None))) },
            
            // UTF8
            new object[] { "a"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None))) },
            new object[] { "C"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('C'), KeyModifiers.Shift))) },
            new object[] { "ñ"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('ñ'), KeyModifiers.None))) },
            new object[] { "\ud800\udf3c"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char("𐌼"), KeyModifiers.None))) },
            
            // Parse CSI U encoded Key code
            new object[] { "\x1B[97u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None))) },
            new object[] { "\x1B[97;2u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.Shift))) },
            new object[] { "\x1B[97;7u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.Alt | KeyModifiers.Control))) },
            new object[] { "\x1B[13u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Enter, KeyModifiers.None))) },
            new object[] { "\x1B[27u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Esc, KeyModifiers.None))) },
            
            new object[] { "\x1B[97;1u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None, KeyEventKind.Press))) },
            new object[] { "\x1B[97;1:1u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None, KeyEventKind.Press))) },
            new object[] { "\x1B[97;5:1u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.Control, KeyEventKind.Press))) },
            new object[] { "\x1B[97;1:2u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None, KeyEventKind.Repeat))) },
            new object[] { "\x1B[97;1:3u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None, KeyEventKind.Release))) },
            
            new object[] { "\x1B[57399u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('0'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57400u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('1'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57401u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('2'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57402u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('3'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57403u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('4'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57404u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('5'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57405u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('6'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57406u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('7'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57407u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('8'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57408u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('9'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57409u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('.'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57410u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('/'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57411u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('*'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57412u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('-'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57413u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('+'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57414u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Enter, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57415u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('='), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57416u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char(','), KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57417u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57418u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Right, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57419u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Up, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57420u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57421u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.PageUp, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57422u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.PageDown, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57423u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Home, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57424u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.End, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57425u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Insert, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57426u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Delete, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            new object[] { "\x1B[57427u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.KeypadBegin, KeyModifiers.None, KeyEventKind.Press, KeyEventState.Keypad))) },
            
            new object[] { "\x1B[57358u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.CapsLock, KeyModifiers.None))) },
            new object[] { "\x1B[57359u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.ScrollLock, KeyModifiers.None))) },
            new object[] { "\x1B[57360u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.NumLock, KeyModifiers.None))) },
            new object[] { "\x1B[57361u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.PrintScreen, KeyModifiers.None))) },
            new object[] { "\x1B[57362u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Pause, KeyModifiers.None))) },
            new object[] { "\x1B[57363u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Menu, KeyModifiers.None))) },
            new object[] { "\x1B[57376u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(13), KeyModifiers.None))) },
            new object[] { "\x1B[57377u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(14), KeyModifiers.None))) },
            new object[] { "\x1B[57378u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(15), KeyModifiers.None))) },
            new object[] { "\x1B[57379u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(16), KeyModifiers.None))) },
            new object[] { "\x1B[57380u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(17), KeyModifiers.None))) },
            new object[] { "\x1B[57381u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(18), KeyModifiers.None))) },
            new object[] { "\x1B[57382u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(19), KeyModifiers.None))) },
            new object[] { "\x1B[57383u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(20), KeyModifiers.None))) },
            new object[] { "\x1B[57384u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(21), KeyModifiers.None))) },
            new object[] { "\x1B[57385u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(22), KeyModifiers.None))) },
            new object[] { "\x1B[57386u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(23), KeyModifiers.None))) },
            new object[] { "\x1B[57387u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(24), KeyModifiers.None))) },
            new object[] { "\x1B[57388u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(25), KeyModifiers.None))) },
            new object[] { "\x1B[57389u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(26), KeyModifiers.None))) },
            new object[] { "\x1B[57390u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(27), KeyModifiers.None))) },
            new object[] { "\x1B[57391u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(28), KeyModifiers.None))) },
            new object[] { "\x1B[57392u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(29), KeyModifiers.None))) },
            new object[] { "\x1B[57393u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(30), KeyModifiers.None))) },
            new object[] { "\x1B[57394u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(31), KeyModifiers.None))) },
            new object[] { "\x1B[57395u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(32), KeyModifiers.None))) },
            new object[] { "\x1B[57396u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(33), KeyModifiers.None))) },
            new object[] { "\x1B[57397u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(34), KeyModifiers.None))) },
            new object[] { "\x1B[57398u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(35), KeyModifiers.None))) },
            new object[] { "\x1B[57428u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.Play), KeyModifiers.None))) },
            new object[] { "\x1B[57429u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.Pause), KeyModifiers.None))) },
            new object[] { "\x1B[57430u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.PlayPause), KeyModifiers.None))) },
            new object[] { "\x1B[57431u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.Reverse), KeyModifiers.None))) },
            new object[] { "\x1B[57432u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.Stop), KeyModifiers.None))) },
            new object[] { "\x1B[57433u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.FastForward), KeyModifiers.None))) },
            new object[] { "\x1B[57434u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.Rewind), KeyModifiers.None))) },
            new object[] { "\x1B[57435u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.TrackNext), KeyModifiers.None))) },
            new object[] { "\x1B[57436u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.TrackPrevious), KeyModifiers.None))) },
            new object[] { "\x1B[57437u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.Record), KeyModifiers.None))) },
            new object[] { "\x1B[57438u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.LowerVolume), KeyModifiers.None))) },
            new object[] { "\x1B[57439u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.RaiseVolume), KeyModifiers.None))) },
            new object[] { "\x1B[57440u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Media(MediaKeyCode.MuteVolume), KeyModifiers.None))) },
            new object[] { "\x1B[57441u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.LeftShift), KeyModifiers.Shift))) },
            new object[] { "\x1B[57442u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.LeftControl), KeyModifiers.Control))) },
            new object[] { "\x1B[57443u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.LeftAlt), KeyModifiers.Alt))) },
            new object[] { "\x1B[57444u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.LeftSuper), KeyModifiers.Super))) },
            new object[] { "\x1B[57445u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.LeftHyper), KeyModifiers.Hyper))) },
            new object[] { "\x1B[57446u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.LeftMeta), KeyModifiers.Meta))) },
            new object[] { "\x1B[57447u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightShift), KeyModifiers.Shift))) },
            new object[] { "\x1B[57448u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightControl), KeyModifiers.Control))) },
            new object[] { "\x1B[57449u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightAlt), KeyModifiers.Alt))) },
            new object[] { "\x1B[57450u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightSuper), KeyModifiers.Super))) },
            new object[] { "\x1B[57451u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightHyper), KeyModifiers.Hyper))) },
            new object[] { "\x1B[57452u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightMeta), KeyModifiers.Meta))) },
            new object[] { "\x1B[57453u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.IsoLevel3Shift), KeyModifiers.None))) },
            new object[] { "\x1B[57454u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.IsoLevel5Shift), KeyModifiers.None))) },
            
            new object[] { "\x1B[57449;3:3u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Modifier(ModifierKeyCode.RightAlt), KeyModifiers.Alt, KeyEventKind.Release))) },
            
            new object[] { "\x1B[97;9u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.Super))) },
            new object[] { "\x1B[97;17u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.Hyper))) },
            new object[] { "\x1B[97;33u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.Meta))) },
            new object[] { "\x1B[97;65u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('a'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.CapsLock))) },
            new object[] { "\x1B[49;129u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('1'), KeyModifiers.None, KeyEventKind.Press, KeyEventState.NumLock))) },
            new object[] { "\x1B[57:40;4u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('('), KeyModifiers.Alt))) },
            new object[] { "\x1B[45:95;4u"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Char('_'), KeyModifiers.Alt))) },
            
            new object[] { "\x1B[;1:3A"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Up, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3B"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3C"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Right, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3D"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Left, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3F"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.End, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3H"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Home, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3P"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(1), KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3Q"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(2), KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3R"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(3), KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[;1:3S"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.F(4), KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[1;1:3B"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.Down, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[5;1:3~"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.PageUp, KeyModifiers.None, KeyEventKind.Release))) },
            new object[] { "\x1B[6;5:3~"u8.ToArray(), Event(Key(new KeyEvent(KeyCode.PageDown, KeyModifiers.Control, KeyEventKind.Release))) },
        };
        
    [Theory]
    [MemberData(nameof(ParseTest))]
    public void Parse(byte[] buffer, object expectedEvent)
    {
        _parse.Advance(buffer, false);
        var @event = _parse.Next();
        @event.Should().NotBeNull();
        @event.Should().Be(expectedEvent);
    }
}
