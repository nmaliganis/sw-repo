using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sw.auth.messaging.Configurations
{
    public class ServiceBusConfig
    {
        public string EndPoint { get; set; }
        public string SasLocator { get; set; }
        public string Topic { get; set; }
    }
}
