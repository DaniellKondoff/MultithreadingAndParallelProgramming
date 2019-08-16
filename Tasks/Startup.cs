using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Startup
    {
        static void Main(string[] args)
        {
            //TaskContinueAndCancellation();
            //WaitingTasks();

            //Task<int> t1 = Task.Factory.StartNew(() => Print(cts.Token), cts.Token, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning, TaskScheduler.Default);
            //Task<int> t2 = Task.Factory.StartNew(() => Print(cts.Token), cts.Token);

            NestedTasks();

            //Console.WriteLine("Main thred is not blocked!");
            //Console.Read();
        }

        private static void NestedTasks()
        {
            Task.Factory.StartNew(() =>
            {
                Task nested = Task.Factory.StartNew(() => Console.WriteLine("Hello World"), TaskCreationOptions.AttachedToParent);
            }).Wait();

            Task t1 = new Task(() =>
            {
                //Task.Run(() => Console.WriteLine("Hello World 2")).Wait();
                Task t2 = new Task(() => Console.WriteLine("Hello World 2"), TaskCreationOptions.AttachedToParent);
                t2.Start();
            });

            t1.Start();

            try
            {
                t1.Wait();
            }
            catch (AggregateException ae)
            {

                ae.Handle(a => true);
            }
        }

        private static void WaitingTasks()
        {
            var cts = new CancellationTokenSource();

            Task<int> t1 = Task.Run(() => Print(cts.Token), cts.Token);
            Task<int> t2 = Task.Run(() => Print(cts.Token), cts.Token);
            Console.WriteLine("Started t1");
            Console.WriteLine("Started t2");

            //t1.Wait();

            //Task.WaitAll(t1, t2);

            //int result = Task.WaitAny(t1, t2);

            var tr = Task.WhenAny(t1, t2);
            tr.ContinueWith(t =>
            {
                Console.WriteLine($"The id of a task which completed first = {tr.Result.Id}");
            });

            Console.WriteLine("After when any");

            Console.WriteLine("Finished t1");
            Console.WriteLine("Finished t2");
        }

        private static void TaskContinueAndCancellation()
        {
            var parentCts = new CancellationTokenSource();
            var childCts = CancellationTokenSource.CreateLinkedTokenSource(parentCts.Token);

            Task<int> t1 = Task.Run(() => Print(parentCts.Token), parentCts.Token);

            Task<int> t3 = Task.Run(() => Print(childCts.Token), childCts.Token);
            Task.Factory.ContinueWhenAll(new[] { t1, t3 }, tasks =>
            {
                var t1Task = tasks[0];
                var t2Task = tasks[1];

                Console.WriteLine($"t1Task: {t1Task.Result}, t2Task: {t2Task.Result}");
            });

            t1.ContinueWith(prevTask =>
            {
                Console.WriteLine($"How many numbers were processed by prev. Task= {prevTask.Result}");
                Task<int> t2 = Task.Run(() => Print(childCts.Token), childCts.Token);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            t1.ContinueWith(t =>
            {
                Console.WriteLine("Finalyy, we are here!");
            }, TaskContinuationOptions.OnlyOnFaulted);

            Thread.Sleep(10);
            parentCts.CancelAfter(10);

            try
            {
                Console.WriteLine($"First task processed: {t1.Result}");
                Console.WriteLine($"Second task processed: {t3.Result}");
            }
            catch (AggregateException ex) { }


            Console.WriteLine($"T1: {t1.Status}");
            Console.WriteLine($"T2: {t3.Status}");


        }

        private static int Print(CancellationToken token)
        {
            //throw new InvalidOperationException();

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

        private static void ErrorHandling()
        {
            var t1 = Task.Run(() => Print(CancellationToken.None));

            try
            {

                t1.Wait();
            }
            catch (AggregateException ae)
            {

                ae.Handle(e =>
                {
                    if (e is InvalidOperationException)
                    {
                        Console.WriteLine("Catch it!!!");
                        return true;
                    }
                    else return false;
                });

            }
        }
    }
}
