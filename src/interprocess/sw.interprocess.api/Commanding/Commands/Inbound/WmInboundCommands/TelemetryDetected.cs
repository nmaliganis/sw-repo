using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Commanding.Events.Inbound;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands
{
    internal class TelemetryDetected : WmInboundCommand
    {
        private readonly string _payload;
        private readonly string _imei;

        public TelemetryDetected(string payload, string imei)
        {
            _payload = payload;
            _imei = imei;
            EventRaisingBehavior = new TelemetryDetectionEventRaising(_payload, _imei);
        }
    }
}