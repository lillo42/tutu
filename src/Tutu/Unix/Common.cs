using System.Diagnostics.CodeAnalysis;

namespace Tutu.Unix;

internal static partial class Unix
{
    private static bool TryToOpen(string file, [NotNullWhen(true)]out FileStream? stream)
    {
        try
        {
            stream = File.Open(file, FileMode.Open);
            return true;
        }
        catch
        {
            stream = null;
            return false;
        }
    }
}