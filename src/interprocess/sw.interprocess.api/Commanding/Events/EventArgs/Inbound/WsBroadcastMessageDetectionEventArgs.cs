namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
    public class WsBroadcastMessageDetectionEventArgs : System.EventArgs
    {
        public string Payload { get; private set; }

        public WsBroadcastMessageDetectionEventArgs(string payload)
        {
            Payload = payload;
        }
    }
}