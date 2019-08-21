namespace SignalingConstructs
{
    public class ProtocolMessage
    {
        public OperationStatus Status { get; }

        public ProtocolMessage(OperationStatus status)
        {
            this.Status = status;
        }
    }

    public enum OperationStatus
    {
        Finished,
        Faulted
    }
}
