using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using System.Collections.Generic;

namespace sw.interprocess.api.Commanding.Commands.Inbound
{
    public interface IWmInboundCommandBuilder
    {
        WmInboundCommand Build(string imei, string header, List<string> commandAttributes);
    }
}