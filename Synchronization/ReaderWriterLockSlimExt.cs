using System;
using System.Threading;

namespace Synchronization
{
    public static class ReaderWriterLockSlimExt
    {
        public static ReaderLockSlimWrapper TakeReaderLock(TimeSpan timeout)
        {
            var rwlock = new ReaderWriterLockSlim();

            bool taken = false;

            try
            {
                taken = rwlock.TryEnterReadLock(timeout);

                if (taken)
                    return new ReaderLockSlimWrapper(rwlock);
                throw new TimeoutException();
            }
            catch (Exception)
            {

                if (taken)
                    rwlock.ExitReadLock();

                throw;
            }
        }
    }
}
