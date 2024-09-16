using sw.interprocess.api.Commanding.PackageRepository;
using System;
using System.Text;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base
{
  public abstract class WmInboundCommandBuilder
  {
    public string Message { get; set; }
    public string ImeiValue { get; set; }
    public string VersionValue { get; set; }
    public abstract void BuildPayload();

    private void BuildHeaders(byte[] wmPackage)
    {
      //VersionValue = ((decimal)wmPackage[WmPackageRepository.GetPackageRepository.HeaderVersionOffset]).ToString();
    }

    protected DateTime FromUnixTime(long unixTime)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
    }

    protected long GetLittleEndianIntegerFromByteFourBytesArray(byte[] data, int startIndex)
    {
      byte[] buffer = new byte[4];
      Array.Copy(data, startIndex, buffer, 0,
          4);

      return buffer[3] << 24
             | buffer[2] << 16
             | buffer[1] << 8
             | buffer[0];
    }
    protected long GetLittleEndianIntegerFromByteTwoBytesArray(byte[] data, int startIndex)
    {
      byte[] buffer = new byte[2];
      Array.Copy(data, startIndex, buffer, 0,
          2);

      return buffer[1] << 8
             | buffer[0];
    }

    protected virtual string BuildMessage(byte[] wmPackage)
    {
      BuildHeaders(wmPackage);
      ExtractImei(wmPackage);
      BuildPayload();
      return Message;
    }
    protected virtual string ExtractImei(byte[] wmPackage)
    {
      byte[] imeiBuffer = new byte[WmPackageRepository.GetPackageRepository.HeaderImeiLength];
      Array.Copy(wmPackage, WmPackageRepository.GetPackageRepository.HeaderImeiOffset, imeiBuffer, 0,
          WmPackageRepository.GetPackageRepository.HeaderImeiLength);
      //Todo: Pass twice GetLittleEndianIntegerFromByteFourBytesArray to Generate LE

      StringBuilder builder = new StringBuilder();
      foreach (var t in imeiBuffer)
      {
        builder.Append(t.ToString("x2"));
      }

      ImeiValue = builder.ToString();
      return ImeiValue;
    }
  }
}
