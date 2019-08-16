using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    public class ErrorHandling
    {
        public async void FetchData()
        {
            using (var wc = new WebClient())
            {
                try
                {
                    string content = await wc.DownloadStringTaskAsync(new Uri("http://www.interact-sw.co.uk/oops/"));
                    Console.WriteLine(content);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:" + e.Message);
                }
            }
        }

        public void TaskSchedularEvent()
        {
            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
            {
                eventArgs.SetObserved();
                ((AggregateException)eventArgs.Exception).Handle(ex =>
                {
                    Console.WriteLine("Exception type: {0}", ex.GetType());
                    return true;
                });
            };
            Run();

            Thread.Sleep(100);
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        private static void Run()
        {
            Task task1 = new Task(() => {
                throw new ArgumentNullException();
            });

            Task task2 = new Task(() => {
                throw new ArgumentOutOfRangeException();
            });

            task1.Start();
            task2.Start();

            while (!task1.IsCompleted || !task2.IsCompleted)
            {
                Thread.Sleep(50);
            }
        }
    }
}
