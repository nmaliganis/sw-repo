using System.Threading.Tasks;

namespace sw.interprocess.api.AIoT
{
    public interface IAIoTConfiguration
    {
        Task ProvisionDevice(string imei);
    }
}