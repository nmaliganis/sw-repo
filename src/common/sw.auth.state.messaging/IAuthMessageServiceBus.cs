using System.Threading.Tasks;

namespace sw.auth.state.messaging
{
    public interface IAuthMessageServiceBus
    {
        Task StopBus();
        Task StartBus();
    }
}