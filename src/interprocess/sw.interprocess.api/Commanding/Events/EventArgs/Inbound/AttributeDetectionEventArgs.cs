namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
    public class AttributeDetectionEventArgs : System.EventArgs
    {
        public string Payload { get; private set; }
        public string Imei { get; private set; }
        public bool Success { get; private set; }

        public AttributeDetectionEventArgs(string payload, bool success, string imei)
        {
            Payload = payload;
            Imei = imei;
            Success = success;
        }
    }
}
