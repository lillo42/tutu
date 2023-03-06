namespace Tutu.Unix.Interop.LibC;

internal struct long_t : IEquatable<long_t>
{
    internal ssize_t __value;
    internal ssize_t Value => __value;

    private long_t(ssize_t value) => __value = value;

    public static implicit operator long(long_t arg) => arg.Value;
    public static explicit operator int(long_t arg) => (int)arg.Value;

    public static explicit operator long_t(long arg) => new(new ssize_t(arg));
    public static implicit operator long_t(int arg) => new(arg);

    public override string ToString() => Value.ToString();

    public override int GetHashCode() => Value.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is long_t v)
        {
            return this == v;
        }

        return false;
    }

    public bool Equals(long_t v) => this == v;

    public static long_t operator +(long_t v) => new(v.Value);
    public static long_t operator -(long_t v) => new(-v.Value);
    public static long_t operator ~(long_t v) => new(~v.Value);
    public static long_t operator ++(long_t v) => new(v.Value + 1);
    public static long_t operator --(long_t v) => new(v.Value - 1);
    public static long_t operator +(long_t v1, long_t v2) => new(v1.Value + v2.Value);
    public static long_t operator -(long_t v1, long_t v2) => new(v1.Value - v2.Value);
    public static long_t operator *(long_t v1, long_t v2) => new(v1.Value * v2.Value);
    public static long_t operator /(long_t v1, long_t v2) => new(v1.Value / v2.Value);
    public static long_t operator %(long_t v1, long_t v2) => new(v1.Value % v2.Value);
    public static long_t operator &(long_t v1, long_t v2) => new(v1.Value & v2.Value);
    public static long_t operator |(long_t v1, long_t v2) => new(v1.Value | v2.Value);
    public static long_t operator ^(long_t v1, long_t v2) => new(v1.Value ^ v2.Value);
    public static long_t operator <<(long_t v, int i) => new(v.Value << i);
    public static long_t operator >> (long_t v, int i) => new(v.Value >> i);
    public static bool operator ==(long_t v1, long_t v2) => v1.Value == v2.Value;
    public static bool operator !=(long_t v1, long_t v2) => v1.Value != v2.Value;
    public static bool operator <(long_t v1, long_t v2) => v1.Value < v2.Value;
    public static bool operator >(long_t v1, long_t v2) => v1.Value > v2.Value;
    public static bool operator <=(long_t v1, long_t v2) => v1.Value <= v2.Value;
    public static bool operator >=(long_t v1, long_t v2) => v1.Value >= v2.Value;
}
