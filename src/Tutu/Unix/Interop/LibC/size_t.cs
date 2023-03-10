namespace Tutu.Unix.Interop.LibC;

internal struct size_t : IEquatable<size_t>
{
    private ulong __value;

    public static implicit operator ulong(size_t arg) => arg.ToUInt64();

    public static explicit operator uint(size_t arg) => arg.ToUInt32();

    public static explicit operator int(size_t arg) => (int) arg.Value;

    public static implicit operator size_t(uint arg) => new(arg);

    public static implicit operator size_t(int arg) => new((uint) arg);

    public static implicit operator size_t(ushort arg) => new((uint)arg);

    public static explicit operator size_t(ulong arg) => new(arg);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value.GetHashCode();

    public override bool Equals(object? obj) => obj is size_t sizeT && this == sizeT;

    public bool Equals(size_t v) => this == v;

    public static size_t operator +(size_t v) => new(v.Value);

    public static size_t operator ~(size_t v) => new(~v.Value);

    public static size_t operator ++(size_t v) => new(v.Value + 1UL);

    public static size_t operator --(size_t v) => new(v.Value - 1UL);

    public static size_t operator +(size_t v1, size_t v2) => new(v1.Value + v2.Value);

    public static size_t operator -(size_t v1, size_t v2) => new(v1.Value - v2.Value);

    public static size_t operator *(size_t v1, size_t v2) => new(v1.Value * v2.Value);

    public static size_t operator /(size_t v1, size_t v2) => new(v1.Value / v2.Value);

    public static size_t operator %(size_t v1, size_t v2) => new(v1.Value % v2.Value);

    public static size_t operator &(size_t v1, size_t v2) => new(v1.Value & v2.Value);

    public static size_t operator |(size_t v1, size_t v2) => new(v1.Value | v2.Value);

    public static size_t operator ^(size_t v1, size_t v2) => new(v1.Value ^ v2.Value);

    public static size_t operator <<(size_t v, int i) => new(v.Value << i);

    public static size_t operator >>(size_t v, int i) => new(v.Value >> i);

    public static bool operator ==(size_t v1, size_t v2) => (long) v1.Value == (long) v2.Value;

    public static bool operator !=(size_t v1, size_t v2) => (long) v1.Value != (long) v2.Value;

    public static bool operator <(size_t v1, size_t v2) => v1.Value < v2.Value;

    public static bool operator >(size_t v1, size_t v2) => v1.Value > v2.Value;

    public static bool operator <=(size_t v1, size_t v2) => v1.Value <= v2.Value;

    public static bool operator >=(size_t v1, size_t v2) => v1.Value >= v2.Value;

    internal ulong Value => this.__value;

    internal size_t(ulong arg) => this.__value = arg;

    internal size_t(uint arg) => this.__value = (ulong) arg;

    internal unsafe size_t(void* arg) => this.__value = (ulong) arg;

    internal size_t(ssize_t arg) => this.__value = (ulong) arg.Value;

    internal uint ToUInt32() => (uint) this.Value;

    internal ulong ToUInt64() => this.Value;
  }
