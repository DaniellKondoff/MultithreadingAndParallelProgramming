using System.Threading;

namespace Synchronization
{
    public class ReaderLockSlimWrapper
    {
        private ReaderWriterLockSlim rwlock;

        public ReaderLockSlimWrapper(ReaderWriterLockSlim rwlock)
        {
            this.rwlock = rwlock;
        }
    }
}