using System.Threading;

namespace Synchronization
{
    public static class Lazy<T> where T : class, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    Interlocked.CompareExchange(ref _instance, new T(), (T)null);
                }

                return _instance;
            }
        }
    }
}
