using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalingConstructs
{
    class Startup
    {
        public static CountdownEvent countDown = new CountdownEvent(3);
        private static Barrier barrier = new Barrier(0);

        static void Main(string[] args)
        {
            
            Console.Read();
        }

        private static void BarrierExample()
        {
            Task[] tasks = new Task[5];

            for (int i = 0; i < 5; i++)
            {
                barrier.AddParticipant();
                int index = i;
                tasks[index] = Task.Factory.StartNew(() =>
                {
                    GetDataAndStoreData(index);
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine("Completed");
        }

        private static void GetDataAndStoreData(int index)
        {
            Console.WriteLine("Getting data from server: " + index);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            barrier.SignalAndWait();

            Console.WriteLine("Send data to server: " + index);

            barrier.SignalAndWait();
        }

        private static void CountDownExample()
        {
            Task.Run(() => { DoWork(); });
            Task.Run(() => { DoWork(); });
            Task.Run(() => { DoWork(); });

            countDown.Wait();
            Console.WriteLine("All task have finished their work;");
        }

        private static void AutoResetAndManualResetEventsExample()
        {
            var bt = new BankTerminal();

            Task purchaseTask = bt.Purchase(100);
            var firstContitue = purchaseTask.ContinueWith(t => { Console.WriteLine("Operation is done!"); });
        }

        private static void DoWork()
        {
            Thread.Sleep(1000);
            Console.WriteLine($"Task with Id: {Task.CurrentId}");
            countDown.Signal();
        }

        private static int GetNumberOfRecors()
        {
            return 10;
        }
    }
}
