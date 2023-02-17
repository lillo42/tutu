namespace Erised;

internal static partial class Unix
{
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
            return new ValueAccess(this);
        }
        
        public class ValueAccess : IDisposable
        {
            private readonly Mutex<T> _mutex;

            public ValueAccess(Mutex<T> mutex)
            {
                _mutex = mutex;
            }

            public T Value
            {
                get => _mutex._value;
                set => _mutex._value  = value;
            }
            

            public static implicit operator T(ValueAccess access)
                => access.Value;

            public void Dispose()
            {
                _mutex._mutex.ReleaseMutex();
            }
        }
    }
}