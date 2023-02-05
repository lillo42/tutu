namespace Erised;

public class Mutex<T>
{
    private readonly Mutex _mutex = new();
    private T _value;

    public Mutex(T value)
    {
        _value = value;
    }

    public ValueAccess Lock()
    {
        _mutex.WaitOne();
        return new ValueAccess(_mutex, _value);
    }
    
    public readonly struct ValueAccess : IDisposable
    {
        private readonly Mutex _mutex;

        public ValueAccess(Mutex mutex, T value)
        {
            _mutex = mutex;
            Value = value;
        }
        
        public T Value { get; }

        public static implicit operator T(ValueAccess access)
                => access.Value;

        public void Dispose()
        {
            _mutex.ReleaseMutex();
        }
    }
}