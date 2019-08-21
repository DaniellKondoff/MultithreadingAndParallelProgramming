using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SignalingConstructs
{
    public class BankTerminal
    {
        private readonly Protocol _protocol;
        private readonly ManualResetEventSlim manualReseteventSlim = new ManualResetEventSlim(false);
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public BankTerminal()
        {
            _protocol = new Protocol();
            _protocol.OnMessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, ProtocolMessage e)
        {
            if(e.Status == OperationStatus.Finished)
            {
                Console.WriteLine("Signaling");
                //this.manualReseteventSlim.Set();
                autoResetEvent.Set();
            }
        }

        public Task Purchase(decimal amount)
        {
            return Task.Run(() =>
            {
                const int purchaseOpCode = 1;
                _protocol.Send(purchaseOpCode, amount);

                //manualReseteventSlim.Reset();
                Console.WriteLine("Waiting for signal.");
                //manualReseteventSlim.Wait();
                autoResetEvent.WaitOne();
            });
        }
    }
}
