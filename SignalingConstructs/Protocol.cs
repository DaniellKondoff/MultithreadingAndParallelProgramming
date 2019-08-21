using System;
using System.Threading;
using System.Threading.Tasks;

namespace SignalingConstructs
{
    public class Protocol
    {
        public event EventHandler<ProtocolMessage> OnMessageReceived;

        public void Send(int opCode, object parameters)
        {
            Task.Run(() =>
            {
                Console.WriteLine("Operation is in action.");
                Thread.Sleep(3000);

                OnMessageReceived?.Invoke(this, new ProtocolMessage(OperationStatus.Finished));
            });
        }
    }
}
