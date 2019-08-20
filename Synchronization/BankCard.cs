using System;
using System.Threading;

namespace Synchronization
{
    public class BankCard
    {
        private decimal _moneyAmount;
        private decimal _credit;
        private readonly object _sync = new object();

        public decimal TotalMoneyAmount
        {
            get
            {
                var rw = new ReaderWriterLockSlim();
                rw.EnterReadLock();

                decimal result = _moneyAmount + _credit;

                rw.ExitReadLock();

                return result;
            }
        }

        public BankCard(decimal moneyAmount)
        {
            this._moneyAmount = moneyAmount;
        }

        public void ReceivePayment(decimal amount)
        {
            //MonitorType(amount);

            var rw = new ReaderWriterLockSlim();

            rw.EnterWriteLock();

            _moneyAmount += amount;

            rw.ExitWriteLock();

        }

        private void MonitorType(decimal amount)
        {
            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(_sync, TimeSpan.FromSeconds(10), ref lockTaken);

                _moneyAmount += amount;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(_sync);
            }

            Monitor.Exit(_sync);
        }

        public void TransferToCard(decimal amount, BankCard recipient)
        {
            //bool lockTaken = false;

            //try
            //{
            //    Monitor.Enter(_sync, ref lockTaken);
            //    _moneyAmount -= amount;
            //    recipient.ReceivePayment(amount);
            //}
            //finally
            //{
            //    if (lockTaken)
            //        Monitor.Exit(_sync);
            //}

            //Monitor.Exit(_sync);

            using (_sync.Lock(TimeSpan.FromSeconds(3)))
            {
                _moneyAmount -= amount;
                recipient.ReceivePayment(amount);
            }
        }
    }
}
