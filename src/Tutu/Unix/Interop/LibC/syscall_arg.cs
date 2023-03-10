namespace Tutu.Unix.Interop.LibC;

internal struct syscall_arg
{
    private ssize_t __value;
    internal ssize_t Value => __value;

    private unsafe syscall_arg(size_t value) => __value = *(ssize_t*)&value;
    private syscall_arg(ssize_t value) => __value = value;

    public static implicit operator syscall_arg(size_t arg) => new(arg);
    public static implicit operator syscall_arg(ssize_t arg) => new(arg);
    public static implicit operator syscall_arg(long_t arg) => new(arg.Value);

    public static implicit operator syscall_arg(uint arg) => new(new size_t(arg));
    public static implicit operator syscall_arg(int arg) => new(new ssize_t(arg));

    public static unsafe implicit operator syscall_arg(void* arg) => new(new size_t(arg));

    public static implicit operator ssize_t(syscall_arg arg) => arg.Value;
    public static explicit operator int(syscall_arg arg) => (int)arg.Value;

    public override string ToString() => Value.ToString();
}
