namespace Tutu.Style.Types;

/// <summary>
/// Represents a foreground or background or underline color.
/// </summary>
/// <param name="Value">An internal value.</param>
/// <param name="Color">The <see cref="Types.Color"/>.</param>
public readonly record struct Colored(int Value, Color Color)
{
    /// <summary>
    /// Creates a foreground instance.
    /// </summary>
    /// <param name="color">The <see cref="Types.Color"/>.</param>
    /// <returns>New instance of <see cref="Colored"/>.</returns>
    public static Colored ForegroundColor(Color color) => new(38, color);

    /// <summary>
    /// Creates a background instance.
    /// </summary>
    /// <param name="color">The <see cref="Types.Color"/>.</param>
    /// <returns>New instance of <see cref="Colored"/>.</returns>
    public static Colored BackgroundColor(Color color) => new(48, color);

    /// <summary>
    /// Creates a underline instance.
    /// </summary>
    /// <param name="color">The <see cref="Types.Color"/>.</param>
    /// <returns>New instance of <see cref="Colored"/>.</returns>
    public static Colored UnderlineColor(Color color) => new(58, color);

    /// <summary>
    /// Parse a <see cref="Colored"/> from a string.
    /// </summary>
    /// <param name="ansi">The ansi string.</param>
    /// <returns>A new instance of <see cref="Colored"/>.</returns>
    public static Colored? ParseAnsi(string ansi)
        => ParseAnsi(ansi.Split(';'));

    /// <summary>
    /// Parse a <see cref="Colored"/> from a string.
    /// </summary>
    /// <param name="ansi">The ansi string.</param>
    /// <returns>A new instance of <see cref="Colored"/>.</returns>
    public static Colored? ParseAnsi(string[] ansi)
    {
        if (ansi.Length > 1)
        {
            var color = Color.ParseAnsi(ansi[1..]) ?? Color.Reset;

            return ansi[0] switch
            {
                "38" or "39" => ForegroundColor(color),
                "48" or "49" => BackgroundColor(color),
                "58" or "59" => UnderlineColor(color),
                _ => new Colored(int.Parse(ansi[0]), color)
            };
        }

        return null;
    }

    /// <summary>
    /// Is this a foreground color.
    /// </summary>
    public bool IsForegroundColor => Value == 38;

    /// <summary>
    /// Is this a background color.
    /// </summary>
    public bool IsBackgroundColor => Value == 48;

    /// <summary>
    /// Is this a underline color.
    /// </summary>
    public bool IsUnderlineColor => Value == 58;

    /// <inheritdoc />
    public override string ToString()
        => Color == Color.Reset ? (Value + 1).ToString() : $"{Value};{Color}";
}
