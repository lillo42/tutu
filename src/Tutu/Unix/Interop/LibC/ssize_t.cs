namespace Tutu.Unix.Interop.LibC;

internal struct ssize_t : IEquatable<ssize_t>
{
    private long __value;

    public static implicit operator long(ssize_t arg) => arg.ToInt64();

    public static explicit operator int(ssize_t arg) => arg.ToInt32();

    public static explicit operator size_t(ssize_t arg) => new(arg);

    public static implicit operator ssize_t(int arg) => new(arg);

    public static explicit operator ssize_t(long arg) => new(arg);

    public override string ToString() => this.Value.ToString();

    public override int GetHashCode() => this.Value.GetHashCode();

    public override bool Equals(object? obj) => obj is ssize_t ssizeT && this == ssizeT;

    public bool Equals(ssize_t v) => this == v;

    public static ssize_t operator +(ssize_t v) => new(v.Value);

    public static ssize_t operator -(ssize_t v) => new(-v.Value);

    public static ssize_t operator ~(ssize_t v) => new(~v.Value);

    public static ssize_t operator ++(ssize_t v) => new(v.Value + 1L);

    public static ssize_t operator --(ssize_t v) => new(v.Value - 1L);

    public static ssize_t operator +(ssize_t v1, ssize_t v2) => new(v1.Value + v2.Value);

    public static ssize_t operator -(ssize_t v1, ssize_t v2) => new(v1.Value - v2.Value);

    public static ssize_t operator *(ssize_t v1, ssize_t v2) => new(v1.Value * v2.Value);

    public static ssize_t operator /(ssize_t v1, ssize_t v2) => new(v1.Value / v2.Value);

    public static ssize_t operator %(ssize_t v1, ssize_t v2) => new(v1.Value % v2.Value);

    public static ssize_t operator &(ssize_t v1, ssize_t v2) => new(v1.Value & v2.Value);

    public static ssize_t operator |(ssize_t v1, ssize_t v2) => new(v1.Value | v2.Value);

    public static ssize_t operator ^(ssize_t v1, ssize_t v2) => new(v1.Value ^ v2.Value);

    public static ssize_t operator <<(ssize_t v, int i) => new(v.Value << i);

    public static ssize_t operator >>(ssize_t v, int i) => new(v.Value >> i);

    public static bool operator ==(ssize_t v1, ssize_t v2) => v1.Value == v2.Value;

    public static bool operator !=(ssize_t v1, ssize_t v2) => v1.Value != v2.Value;

    public static bool operator <(ssize_t v1, ssize_t v2) => v1.Value < v2.Value;

    public static bool operator >(ssize_t v1, ssize_t v2) => v1.Value > v2.Value;

    public static bool operator <=(ssize_t v1, ssize_t v2) => v1.Value <= v2.Value;

    public static bool operator >=(ssize_t v1, ssize_t v2) => v1.Value >= v2.Value;

    internal long Value => this.__value;

    internal ssize_t(long arg) => this.__value = arg;

    internal ssize_t(int arg) => this.__value = (long) arg;

    internal int ToInt32() => (int) this.Value;

    internal long ToInt64() => this.Value;
  }
