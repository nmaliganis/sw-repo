using Microsoft.Azure.Devices.Client;

namespace sw.interprocess.api.Helpers.Infrastructure
{
  public interface IConfigAIoT
  {
    public string IdScope { get; }
    public string PrimaryKey { get; }
    public string GlobalDeviceEndpoint { get; }
    public TransportType TransportType { get; }
  }
}