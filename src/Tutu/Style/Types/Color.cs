namespace Tutu.Style.Types;

/// <summary>
/// Represents a color.
/// </summary>
/// <remarks>
/// <para><strong>Platform-specific Notes.</strong></para>
/// <para>The following list of 16 base colors are available for almost all terminals (Windows 7 and 8 included).</para>
/// <list type="table">
///     <listheader>
///         <term>Light</term>
///         <term>Dark</term>
///     </listheader>
///     <item>
///         <term>DarkGrey</term>
///         <term>Black</term>
///     </item>
///     <item>
///         <term>Red</term>
///         <term>DarkRed</term>
///     </item>
///     <item>
///         <term>Green</term>
///         <term>DarkGreen</term>
///     </item>
///     <item>
///         <term>Yellow</term>
///         <term>DarkYellow</term>
///     </item>
///     <item>
///         <term>Blue</term>
///         <term>DarkBlue</term>
///     </item>
///     <item>
///         <term>Magenta</term>
///         <term>DarkMagenta</term>
///     </item>
///     <item>
///         <term>Cyan</term>
///         <term>DarkCyan</term>
///     </item>
///     <item>
///         <term>White</term>
///         <term>Grey</term>
///     </item>
/// </list>
/// <para>Most UNIX terminals and Windows 10 consoles support additional colors.</para>
/// <para>See <see cref="Rgb"/> or <see cref="AnsiValue"/> for more info.</para>
/// </remarks>
public readonly record struct Color(string Name, byte[] Values)
{
    /// <summary>
    /// Reset color.
    /// </summary>
    public static Color Reset { get; } = new("reset", new byte[] { 0 });

    /// <summary>
    /// Black color.
    /// </summary>
    public static Color Black { get; } = new("black", new byte[] { 5, 0 });

    /// <summary>
    /// Dark grey color.
    /// </summary>
    public static Color DarkGrey { get; } = new("dark-grey", new byte[] { 5, 8 });

    /// <summary>
    /// Red color.
    /// </summary>
    public static Color Red { get; } = new("red", new byte[] { 5, 9 });

    /// <summary>
    /// Dark red color.
    /// </summary>
    public static Color DarkRed { get; } = new("dark-red", new byte[] { 5, 1 });

    /// <summary>
    /// Green color.
    /// </summary>
    public static Color Green { get; } = new("green", new byte[] { 5, 10 });

    /// <summary>
    /// Dark green color.
    /// </summary>
    public static Color DarkGreen { get; } = new("dark-green", new byte[] { 5, 2 });

    /// <summary>
    /// Yellow color.
    /// </summary>
    public static Color Yellow { get; } = new("yellow", new byte[] { 5, 11 });

    /// <summary>
    /// Dark yellow color.
    /// </summary>
    public static Color DarkYellow { get; } = new("dark-yellow", new byte[] { 5, 3 });

    /// <summary>
    /// Blue color.
    /// </summary>
    public static Color Blue { get; } = new("blue", new byte[] { 5, 12 });

    /// <summary>
    /// Dark blue color.
    /// </summary>
    public static Color DarkBlue { get; } = new("dark-blue", new byte[] { 5, 4 });

    /// <summary>
    /// Magenta color.
    /// </summary>
    public static Color Magenta { get; } = new("magenta", new byte[] { 5, 13 });

    /// <summary>
    /// Dark magenta color.
    /// </summary>
    public static Color DarkMagenta { get; } = new("dark-magenta", new byte[] { 5, 5 });

    /// <summary>
    /// Cyan color.
    /// </summary>
    public static Color Cyan { get; } = new("cyan", new byte[] { 5, 14 });

    /// <summary>
    /// Dark cyan color.
    /// </summary>
    public static Color DarkCyan { get; } = new("dark-cyan", new byte[] { 5, 6 });

    /// <summary>
    /// White color.
    /// </summary>
    public static Color White { get; } = new("white", new byte[] { 5, 15 });

    /// <summary>
    /// Dark grey color.
    /// </summary>
    public static Color Grey { get; } = new("grey", new byte[] { 5, 7 });

    internal static Color[] AllColors { get; } =
    {
        Reset,
        Black,
        DarkGrey,
        Red,
        DarkRed,
        Green,
        DarkGreen,
        Yellow,
        DarkYellow,
        Blue,
        DarkBlue,
        Magenta,
        DarkMagenta,
        Cyan,
        DarkCyan,
        White,
        Grey,
    };

    /// <summary>
    /// Creates a new RGB color.
    /// </summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    /// <returns>The new RGB color.</returns>
    public static Color Rgb(byte red, byte green, byte blue) => new($"RGB {{ r: {red}, g: {green}, b: {blue} }}", new byte[] { 2, red, green, blue });

    /// <summary>
    /// Creates a new ANSI color.
    /// </summary>
    /// <param name="value">The ANSI value.</param>
    /// <returns>The new ANSI color.</returns>
    public static Color AnsiValue(byte value) => new($"AnsiValue({value})", new byte[] { 5, value });

    /// <summary>
    /// Try to parse a color from a string.
    /// </summary>
    /// <param name="source">The color name.</param>
    /// <param name="color">The <see cref="Color"/>.</param>
    /// <returns>True if could convert, otherwise return false.</returns>
    public static bool TryFrom(string source, out Color color)
    {
        var sourceLower = source.ToLowerInvariant();
        if (sourceLower == Black.Name)
        {
            color = Black;
            return true;
        }

        if (sourceLower == DarkGrey.Name)
        {
            color = DarkGrey;
            return true;
        }

        if (sourceLower == Red.Name)
        {
            color = Red;
            return true;
        }

        if (sourceLower == DarkRed.Name)
        {
            color = DarkRed;
            return true;
        }

        if (sourceLower == Green.Name)
        {
            color = Green;
            return true;
        }

        if (sourceLower == DarkGreen.Name)
        {
            color = DarkGreen;
            return true;
        }

        if (sourceLower == Yellow.Name)
        {
            color = Yellow;
            return true;
        }

        if (sourceLower == DarkYellow.Name)
        {
            color = DarkYellow;
            return true;
        }

        if (sourceLower == Blue.Name)
        {
            color = Blue;
            return true;
        }

        if (sourceLower == DarkBlue.Name)
        {
            color = DarkBlue;
            return true;
        }

        if (sourceLower == Magenta.Name)
        {
            color = Magenta;
            return true;
        }

        if (sourceLower == DarkMagenta.Name)
        {
            color = DarkMagenta;
            return true;
        }

        if (sourceLower == Cyan.Name)
        {
            color = Cyan;
            return true;
        }

        if (sourceLower == DarkCyan.Name)
        {
            color = DarkCyan;
            return true;
        }

        if (sourceLower == White.Name)
        {
            color = White;
            return true;
        }

        if (sourceLower == Grey.Name)
        {
            color = Grey;
            return true;
        }

        color = White;
        return false;
    }

    /// <summary>
    /// Parse a color from a string.
    /// </summary>
    /// <param name="source">The color name.</param>
    /// <returns>The <see cref="Color"/>.</returns>
    /// <exception cref="ArgumentException">When the source name does not match.</exception>
    public static Color From(string source)
    {
        if (TryFrom(source, out var color))
        {
            return color;
        }

        throw new ArgumentException("Invalid color name", nameof(source));
    }

    /// <summary>
    /// The logic for ParseAnsi, takes an iterator of the sequences terms (the numbers between the
    /// ';'). It's a separate function so it can be used by both <see cref="ParseAnsi(string[])"/> and
    /// <see cref="Colored.ParseAnsi(string)"/>.
    /// </summary>
    /// <param name="values">The source value.</param>
    /// <returns>The <see cref="Color"/>.</returns>
    public static Color? ParseAnsi(string[] values)
    {
        Color? color = null;
        return values switch
        {
            // 8 bit colors: 5;<n>
            ["5", _] when byte.TryParse(values[1], out var value) => AnsiValue(value),
            // 24 bits colors `2;<r>;<g>;<b>`
            ["2", _, _, _] when byte.TryParse(values[1], out var red) &&
                                byte.TryParse(values[2], out var green) &&
                                byte.TryParse(values[3], out var blue) => Rgb(red, green, blue),
            _ => color
        };
    }


    /// <summary>
    /// Parses an ANSI color sequence.
    /// </summary>
    /// <returns>The <see cref="Color"/>.</returns>
    /// <remarks>
    /// Currently, 3/4 bit color values aren't supported so return <see langword="null"/>.
    /// </remarks>
    public static Color? ParseAnsi(string ansi)
        => ParseAnsi(ansi.Split(";"));

    /// <inheritdoc />
    public override string ToString() => string.Join(";", Values);
}
