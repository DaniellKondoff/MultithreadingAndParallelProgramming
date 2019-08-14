using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Startup
    {
        static void Main(string[] args)
        {
            var parentCts = new CancellationTokenSource();
            var childCts = CancellationTokenSource.CreateLinkedTokenSource(parentCts.Token);

            Task<int> t1 = Task.Run(() => Print(parentCts.Token), parentCts.Token);
            Task<int> t2 = Task.Run(() => Print(childCts.Token), childCts.Token);

            //Task<int> t1 = Task.Factory.StartNew(() => Print(cts.Token), cts.Token, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning, TaskScheduler.Default);
            //Task<int> t2 = Task.Factory.StartNew(() => Print(cts.Token), cts.Token);

            //Thread.Sleep(10);
            parentCts.CancelAfter(10);

            try
            {
                Console.WriteLine($"First task processed: {t1.Result}");
                Console.WriteLine($"Second task processed: {t2.Result}");
            }
            catch (AggregateException ex) { }


            Console.WriteLine($"T1: {t1.Status}");
            Console.WriteLine($"T2: {t2.Status}");

            Console.Read();
        }

        private static int Print(CancellationToken token)
        {
            Console.WriteLine($"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
            int total = 0;

            for (int i = 0; i < 100; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancellation Requested");
                }

                token.ThrowIfCancellationRequested();
                total++;
                Console.WriteLine($"Current task id = {Task.CurrentId} value = {i}");
            }

            return total;
        }
    }
}
