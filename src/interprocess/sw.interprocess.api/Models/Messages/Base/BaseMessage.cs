using System;

namespace sw.interprocess.api.Models.Messages.Base
{
    public abstract class BaseMessage
    {
        public string Imei { get; set; }
        public DateTime Date { get; set; }
        public string Value { get; set; }
        public string Header { get; set; }
    }
}