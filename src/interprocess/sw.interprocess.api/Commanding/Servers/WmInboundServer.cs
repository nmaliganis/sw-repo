using sw.interprocess.api.Commanding.Servers.Base;

namespace sw.interprocess.api.Commanding.Servers
{
    public sealed class WmInboundServer : WmInboundBaseServer
    {
        private WmInboundServer()
        {

        }
        public static WmInboundServer GetWmInboundServer { get; } = new WmInboundServer();
    }
}
