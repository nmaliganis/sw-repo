using sw.interprocess.api.Commanding.Servers;
using Quartz;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace sw.interprocess.api.Schedulers
{
    public class KeepAliveWsInitializerJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            WmInboundServer.GetWmInboundServer.RaiseWsBroadcastAckMessageDetection(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
            return Task.CompletedTask;
        }
    }
}