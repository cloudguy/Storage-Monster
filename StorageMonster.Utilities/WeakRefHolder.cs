using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Utilities
{
    public class WeakRefHolder<T> where T : class
    {
        private Func<T> _referenceCreator;
        private object _locker = new object();
        private WeakReference _weakReferencee = new WeakReference(null);

        public T Target
        {
            get { return GetReference(); }
        }

        public WeakRefHolder(Func<T> referenceCreator)
        {
            _referenceCreator = referenceCreator;
        }

        public T GetReference()
        {
            lock (_locker)
            {
                T target = _weakReferencee.Target as T;
                if (target != null) return target;

                if (_referenceCreator == null) 
                    throw new InvalidOperationException("Reference creator was not set");

                target = _referenceCreator();
                if (target == null)
                    throw new InvalidOperationException("Reference creator returned null");

                _weakReferencee.Target = target;
                return target;
            }
        }
    }
}
