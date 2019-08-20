using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
    class Startup
    {
        public static SemaphoreSlim Bouncer { get; set; }

        static void Main(string[] args)
        {
            Bouncer = new SemaphoreSlim(3, 3);

            OpenNightClub();

            Thread.Sleep(20000);
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
            Bouncer.Release(1);

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
    }
}
