namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
    public class WsBroadcastAckMessageDetectionEventArgs : System.EventArgs
    {
        public string Payload { get; private set; }

        public WsBroadcastAckMessageDetectionEventArgs(string payload)
        {
            Payload = payload;
        }
    }
}