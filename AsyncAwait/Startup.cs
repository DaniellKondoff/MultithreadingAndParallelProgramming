using System;
using System.Net;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Startup
    {
        private void DumpWebPage(string uri)
        {
            WebClient wc = new WebClient();
            string page = wc.DownloadString(uri);
            Console.WriteLine(page);
        }

        private async Task DumpWebPageAsync(string uri)
        {
            WebClient wc = new WebClient();
            string page = await wc.DownloadStringTaskAsync(uri);
            Console.WriteLine(page);
        }

        private void DumbWebPageTaskBased(string uri)
        {
            WebClient wc = new WebClient();
            var task = wc.DownloadStringTaskAsync(uri);
            task.ContinueWith(t =>
            {
                Console.WriteLine(t.Result);
            });
        }

        async static Task Catcher()
        {
            try
            {
                Task thower = Thrower();
                await thower;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
            }
        }

        async static Task Thrower()
        {
            await Task.Delay(100);
            throw new InvalidOperationException();
        }

        static void Main(string[] args)
        {
            
            Console.Read();
        }
    }
}
