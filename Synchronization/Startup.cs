using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
    class Startup
    {
        public static SemaphoreSlim Bouncer { get; set; }
        private static SemaphoreSlim semaphore;
        private static int padding;

        static void Main(string[] args)
        {
            //Bouncer = new SemaphoreSlim(3, 3);

            //OpenNightClub();

            //Thread.Sleep(20000);

            SemaphoreSlimExample();
            Console.Read();
        }

        private static void OpenNightClub()
        {
            for (int i = 1; i <= 50; i++)
            {
                var number = i;
                Task.Run(() => Guest(number));
            }
        }

        private static void Guest(int guestNumber)
        {
            Console.WriteLine($"Gust {guestNumber} is waitng to entering nighclub");
            Bouncer.Wait();

            Console.WriteLine($"Guest {guestNumber} is doing some dancing.");
            Thread.Sleep(500);

            Console.WriteLine($"Guest {guestNumber} is leaving the nightclub.");
            Bouncer.Release(3);

            Console.WriteLine();
        }

        private static void InterLockExample()
        {
            Character c = new Character();

            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                Task t1 = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        c.CastArmorSpell(true);
                    }
                });

                tasks.Add(t1);

                Task t2 = Task.Factory.StartNew(() =>
                {
                    for (int k = 0; k < 10; k++)
                    {
                        c.CastArmorSpell(false);
                    }
                });

                tasks.Add(t2);
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Result is: {c.Armor}");
        }

        public static void SemaphoreSlimExample()
        {
            semaphore = new SemaphoreSlim(0, 3);
            Console.WriteLine("{0} tasks can enter the semaphore.", semaphore.CurrentCount);

            Task[] tasks = new Task[5];

            // Create and start five numbered tasks.
            for (int i = 0; i <= 4; i++)
            {
                tasks[i] = Task.Run(() => {
                    // Each task begins by requesting the semaphore.
                    Console.WriteLine("Task {0} begins and waits for the semaphore.", Task.CurrentId);
                    semaphore.Wait();

                    Interlocked.Add(ref padding, 100);

                    Console.WriteLine("Task {0} enters the semaphore.", Task.CurrentId);

                    // The task just sleeps for 1+ seconds.
                    Thread.Sleep(1000 + padding);

                    Console.WriteLine("Task {0} releases the semaphore; previous count: {1}.", Task.CurrentId, semaphore.Release());
                });
            }

            // Wait for half a second, to allow all the tasks to start and block.
            Thread.Sleep(500);

            // Restore the semaphore count to its maximum value.
            Console.Write("Main thread calls Release(3) --> ");
            semaphore.Release(3);
            Console.WriteLine("{0} tasks can enter the semaphore.", semaphore.CurrentCount);
            // Main thread waits for the tasks to complete.
            Task.WaitAll(tasks);

            Console.WriteLine("Main thread exits.");
        }
    }
}
