namespace CloudBin.Core.Utilities
{
    public sealed class Synchronized<T>
    {
        private readonly object _locker;
        private T _valueInternal;

        public Synchronized()
        {
            _locker = new object();
        }

        public Synchronized(T value)
            :this(value, new object())
        {
        }

        public Synchronized(T value, object Lock)
        {
            _locker = Lock;
            Value = value;
        }

        public T Value
        {
            get
            {
                lock (_locker)
                {
                    return _valueInternal;
                }
            }
            set
            {
                lock (_locker)
                {
                    _valueInternal = value;
                }
            }
        }
    }
}
