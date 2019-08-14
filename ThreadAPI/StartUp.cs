using System;
using System.Threading;

namespace ThreadAPI
{
    class StartUp
    {
        static void Main(string[] args)
        {
            var printInfo = new PrintInfo();

            Thread t1 = new Thread(() => Print(false, printInfo));
            t1.Start();

            //Print(true, printInfo);
        }

        private static void Print(bool isEven, PrintInfo printInfo)
        {
            Console.WriteLine($"Current Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            if (isEven)
            {
                for (int i = 0; i < 100; i++)
                {
                    if (i % 2 == 0)
                    {
                        printInfo.ProcessedNumbers++;
                        Console.WriteLine(i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 100; i++)
                {
                    if (i % 2 != 0)
                    {
                        printInfo.ProcessedNumbers++;
                        Console.WriteLine(i);
                    }
                }
            }
            
        }
    }
}
