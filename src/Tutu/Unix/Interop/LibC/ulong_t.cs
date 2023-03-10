namespace Tutu.Unix.Interop.LibC;

internal struct ulong_t : IEquatable<ulong_t>
{
    internal size_t __value;

    internal size_t Value => this.__value;

    private ulong_t(size_t value) => this.__value = value;

    public static implicit operator ulong(ulong_t arg) => (ulong) arg.Value;

    public static explicit operator uint(ulong_t arg) => (uint) arg.Value;

    public static explicit operator ulong_t(ulong arg) => new(new size_t(arg));

    public static implicit operator ulong_t(uint arg) => new((size_t) arg);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value.GetHashCode();

    public override bool Equals(object? obj) => obj is ulong_t ulongT && this == ulongT;

    public bool Equals(ulong_t v) => this == v;

    public static ulong_t operator +(ulong_t v) => new(v.Value);

    public static ulong_t operator ~(ulong_t v) => new(~v.Value);

    public static ulong_t operator ++(ulong_t v) => new(v.Value + (size_t) 1);

    public static ulong_t operator --(ulong_t v) => new(v.Value - (size_t) 1);

    public static ulong_t operator +(ulong_t v1, ulong_t v2) => new(v1.Value + v2.Value);

    public static ulong_t operator -(ulong_t v1, ulong_t v2) => new(v1.Value - v2.Value);

    public static ulong_t operator *(ulong_t v1, ulong_t v2) => new(v1.Value * v2.Value);

    public static ulong_t operator /(ulong_t v1, ulong_t v2) => new(v1.Value / v2.Value);

    public static ulong_t operator %(ulong_t v1, ulong_t v2) => new(v1.Value % v2.Value);

    public static ulong_t operator &(ulong_t v1, ulong_t v2) => new(v1.Value & v2.Value);

    public static ulong_t operator |(ulong_t v1, ulong_t v2) => new(v1.Value | v2.Value);

    public static ulong_t operator ^(ulong_t v1, ulong_t v2) => new(v1.Value ^ v2.Value);

    public static ulong_t operator <<(ulong_t v, int i) => new(v.Value << i);

    public static ulong_t operator >>(ulong_t v, int i) => new(v.Value >> i);

    public static bool operator ==(ulong_t v1, ulong_t v2) => v1.Value == v2.Value;

    public static bool operator !=(ulong_t v1, ulong_t v2) => v1.Value != v2.Value;

    public static bool operator <(ulong_t v1, ulong_t v2) => v1.Value < v2.Value;

    public static bool operator >(ulong_t v1, ulong_t v2) => v1.Value > v2.Value;

    public static bool operator <=(ulong_t v1, ulong_t v2) => v1.Value <= v2.Value;

    public static bool operator >=(ulong_t v1, ulong_t v2) => v1.Value >= v2.Value;
}
