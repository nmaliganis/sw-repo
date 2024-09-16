using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Commanding.Events.Inbound;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands
{
    internal class TelemetryRowDetected : WmInboundCommand
    {
        private readonly string _payload;
        private readonly string _imei;

        public TelemetryRowDetected(string payload, string imei)
        {
            _payload = payload;
            _imei = imei;
            EventRaisingBehavior = new TelemetryRowDetectionEventRaising(_payload, _imei);
        }
    }
}