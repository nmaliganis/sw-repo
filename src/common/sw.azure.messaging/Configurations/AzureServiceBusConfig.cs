namespace sw.azure.messaging.Configurations
{
    public class AzureServiceBusConfig
    {
        public string EndPoint { get; set; }
        public string SasLocator { get; set; }
        public string Topic { get; set; }
        public string TopicDriver { get; set; }
        public string TopicError { get; set; }
    }
}