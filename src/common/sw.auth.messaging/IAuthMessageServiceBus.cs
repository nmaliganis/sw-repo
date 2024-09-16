using System.Threading.Tasks;
using sw.auth.messaging.Configurations;

namespace sw.auth.messaging
{
    public interface IAuthMessageServiceBus
    {
        void CreatePublisher(ServiceBusConfig config);
        void StopBus();
        void StartBus();
    }
}