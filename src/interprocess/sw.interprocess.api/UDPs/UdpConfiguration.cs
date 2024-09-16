using Coldairarrow.DotNettySocket;
using sw.interprocess.api.Commanding.Commands.Inbound;
using sw.interprocess.api.Commanding.PackageCheckers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;
using sw.interprocess.api.Commanding.Listeners.Inbounds;
using sw.interprocess.api.Commanding.PackageRepository;
using sw.interprocess.api.Commanding.Servers;

namespace sw.interprocess.api.UDPs
{
  public class UdpConfiguration : IUdpConfiguration, ITelemetryDetectionActionListener,
    ITelemetryRowDetectionActionListener, IAttributeDetectionActionListener
  {
    private IUdpSocket _theServer;
    private EndPoint _point;
    private IDictionary<string, EndPoint> _points;

    public UdpConfiguration()
    {
      _points = new Dictionary<string, EndPoint>();
      WmInboundServer.GetWmInboundServer.Attach((ITelemetryDetectionActionListener)this);
      WmInboundServer.GetWmInboundServer.Attach((ITelemetryRowDetectionActionListener)this);
      WmInboundServer.GetWmInboundServer.Attach((IAttributeDetectionActionListener)this);
    }

    public async void EstablishConnection()
    {
      _theServer = await SocketBuilderFactory.GetUdpSocketBuilder(6002)
        .OnClose(server =>
        {
          Log.Information($"Server closed");
        })
        .OnException(ex =>
        {
          Log.Error($"Server exception:{ex.Message}");
        })
        .OnRecieve((server, point, bytes) =>
        {
          Log.Information($"Server:Received from[{point.ToString()}]data:{Encoding.UTF8.GetString(bytes)}");

          byte[] imeiBuffer = new byte[WmPackageRepository.GetPackageRepository.HeaderImeiLength];
          Array.Copy(bytes, WmPackageRepository.GetPackageRepository.HeaderImeiOffset, imeiBuffer, 0,
            WmPackageRepository.GetPackageRepository.HeaderImeiLength);

          StringBuilder builder = new StringBuilder();
          foreach (var t in imeiBuffer)
          {
            builder.Append(t.ToString("x2"));
          }

          string convertedImeiBuffer = builder.ToString();

          if (!_points.ContainsKey(convertedImeiBuffer))
            _points.Add(convertedImeiBuffer, point);

          _point = point;

          // if (PackageChecker.Checker.IsValidPackage(bytes))
          // {
          //   PackageChecker.Checker.Check(bytes);
          //
          //   // WmInboundCommandBuilderRepository.GetCommandBuilderRepository
          //   //     [bytes[WmPackageRepository.GetPackageRepository.HeaderMessageTypeOffset]]
          //   //   .Build(bytes).RaiseWmEvent(WmInboundServer.GetWmInboundServer);
          // }

        })
        .OnSend((server, point, bytes) =>
        {
          Log.Information(
            $"Server sends data:aims[{point.ToString()}]data:{Encoding.UTF8.GetString(bytes)}");
        })
        .OnStarted(server =>
        {
          Log.Information($"Server startup");
        }).BuildAsync();
    }

    public void Update(object sender, TelemetryDetectionEventArgs e)
    {
      if (e.Success)
      {
        //Todo: Keep list track of registered Udp Point 
        if (_point != null)
        {
          _theServer.Send(new byte[] { 0x01, 0xA0 }, _points[e.Imei]);
          //this._theServer.Send(new byte[] {0x01, 0xA0}, _point);
        }

        try
        {
          //Todo: Mqtt instead communication
        }
        catch (Exception ex)
        {
          Log.Error("An error occured during .." + ex.Message);
        }
      }

      Log.Information($"Update for ITelemetryDetectionActionListener was caught");
    }

    public void Update(object sender, AttributeDetectionEventArgs e)
    {
      if (e.Success)
      {
        //Todo: Keep list track of registered Udp Point 
        if (_point != null)
        {
          _theServer.Send(new byte[] { 0x01, 0xA0 }, _points[e.Imei]);
        }
      }

      Log.Information($"Update for IAttributeDetectionActionListener was caught");
    }

    public void Update(object sender, TelemetryRowDetectionEventArgs e)
    {
      if (e.Success)
      {
        //Todo: Keep list track of registered Udp Point 
        if (_point != null)
        {
          foreach (var endPoint in _points)
          {
            if (endPoint.Key.Contains(e.Imei))
            {
              //this._theServer.Send(new byte[] {0x01, 0xA0}, endPoint.Value);
            }
          }

          _theServer.Send(new byte[] { 0x01, 0xA0 }, _points[e.Imei]);
          //this._theServer.Send(new byte[] {0x01, 0xA0}, _point);
        }

        try
        {
          //Todo: Mqtt instead communication
        }
        catch (Exception ex)
        {
          Log.Error("An error occured during .." + ex.Message);
        }
      }

      Log.Information($"Update for ITelemetryRowDetectionActionListener was caught");
    }
  }
}
