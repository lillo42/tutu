namespace Tutu.Unix.Interop.LibC;

internal struct mode_t : IEquatable<mode_t>
{
    private uint __value;

    internal uint Value => this.__value;

    private mode_t(uint value) => this.__value = value;

    public static implicit operator mode_t(ushort arg) => new(arg);

    public static explicit operator ushort(mode_t arg) => (ushort) arg.Value;

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value.GetHashCode();

    public override bool Equals(object? obj) => obj is mode_t modeT && this == modeT;

    public bool Equals(mode_t v) => this == v;

    public static mode_t operator ~(mode_t v) => new(~v.Value);

    public static mode_t operator &(mode_t v1, mode_t v2) => new(v1.Value & v2.Value);

    public static mode_t operator |(mode_t v1, mode_t v2) => new(v1.Value | v2.Value);

    public static bool operator ==(mode_t v1, mode_t v2) => (int) v1.Value == (int) v2.Value;

    public static bool operator !=(mode_t v1, mode_t v2) => (int) v1.Value != (int) v2.Value;
}
