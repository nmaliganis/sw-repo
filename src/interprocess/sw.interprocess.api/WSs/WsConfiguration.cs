using Coldairarrow.DotNettySocket;
using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;
using sw.interprocess.api.Commanding.Listeners.Inbounds;
using sw.interprocess.api.Commanding.Servers;
using sw.azure.messaging.Events;
using MassTransit;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sw.interprocess.api.WSs
{
    public class WsConfiguration : IWsConfiguration,
        ITelemetryDetectionActionListener,
        IWsBroadcastMessageDetectionActionListener,
        IWsBroadcastAckMessageDetectionActionListener,
        IAttributeDetectionActionListener,
        IUltrasonicDetectionActionListener,
        IGPSDetectionActionListener,
        IDigitalDetectionActionListener
    {
        private IWebSocketServer _theServer;
        private readonly List<IWebSocketConnection> _theConnections;
        private readonly IBus _bus;


        public WsConfiguration(IBus bus)
        {
            this._theConnections = new List<IWebSocketConnection>();
            this._bus = bus;

            WmInboundServer.GetWmInboundServer.Attach((ITelemetryDetectionActionListener)this);
            WmInboundServer.GetWmInboundServer.Attach((IAttributeDetectionActionListener)this);
            WmInboundServer.GetWmInboundServer.Attach((IUltrasonicDetectionActionListener)this);
            WmInboundServer.GetWmInboundServer.Attach((IGPSDetectionActionListener)this);
            WmInboundServer.GetWmInboundServer.Attach((IDigitalDetectionActionListener)this);
            WmInboundServer.GetWmInboundServer.Attach((IWsBroadcastMessageDetectionActionListener)this);
            WmInboundServer.GetWmInboundServer.Attach((IWsBroadcastAckMessageDetectionActionListener)this);
        }

        public async void EstablishConnection()
        {
            _theServer = await SocketBuilderFactory.GetWebSocketServerBuilder(6001)
              .OnConnectionClose((server, connection) =>
              {
                  var alreadyConnected = _theConnections.FirstOrDefault(x => x.ConnectionId == connection.ConnectionId);
                  if (alreadyConnected != null)
                  {
                      Log.Information(
                  $"This connection already exists :{connection.ConnectionName},Current connection number:{server.GetConnectionCount()}");
                      try
                      {
                          _theConnections.Remove(connection);
                      }
                      catch (Exception e)
                      {
                          Log.Fatal(
                      $"OnConnectionClose:{e.Message}"
                    );
                      }

                  }
                  Log.Fatal(
              $"Connection cannot close cause not exists," +
              $"Connection name[{connection.ConnectionName}],Current connection number:{server.GetConnectionCount()}"
              );
              })
              .OnException(ex => { Log.Error($"Server exception:{ex.Message}"); })
              .OnNewConnection((server, connection) =>
              {
                  connection.ConnectionName = $"1st Name{connection.ConnectionId}";
                  var alreadyConnected = _theConnections.FirstOrDefault(x => x.ConnectionId == connection.ConnectionId);
                  if (alreadyConnected != null)
                  {
                      Log.Fatal(
                  $"This connection already exists :{connection.ConnectionName},Current connection number:{server.GetConnectionCount()}");
                  }
                  else
                  {
                      Log.Information(
                  $"New connection:{connection.ConnectionName},Current connection number:{server.GetConnectionCount()}");
                      try
                      {
                          _theConnections.Add(connection);
                      }
                      catch (Exception e)
                      {
                          Log.Fatal(
                      $"OnNewConnection:{e.Message}"
                    );
                      }
                  }

              })
              .OnRecieve((server, connection, msg) =>
              {
                  connection?.Send("ACK");
                  Log.Information($"Server:Data{msg}");
              })
              .OnSend((server, connection, msg) =>
              {
                  Log.Information($"Connection name[{connection.ConnectionName}]send Daa:{msg}");
              })
              .OnServerStarted(server => { Log.Information($"Service Start"); }).BuildAsync();
        }

        public void Update(object sender, TelemetryDetectionEventArgs e)
        {
            foreach (var theConnection in _theConnections)
            {
                theConnection?.Send(e.Payload);
                Log.Information($"Update for ITelemetryDetectionActionListener was caught");
            }
        }

        public void Update(object sender, AttributeDetectionEventArgs e)
        {
            foreach (var theConnection in _theConnections)
            {
                theConnection?.Send(e.Payload);
                Log.Information($"Update for IAttributeDetectionActionListener was caught");
            }
        }

        public void Update(object sender, WsBroadcastMessageDetectionEventArgs e)
        {
            Log.Information($"Update for WsBroadcastMessageDetectionEventArgs Before at :{DateTime.UtcNow}");
            foreach (var theConnection in _theConnections)
            {
                theConnection?.Send(e.Payload);
            }
            Log.Information($"Update for WsBroadcastMessageDetectionEventArgs After at :{DateTime.UtcNow}");
        }

        public void Update(object sender, WsBroadcastAckMessageDetectionEventArgs e)
        {
            if (_theConnections != null)
                foreach (var theConnection in _theConnections)
                {
                    theConnection?.Send($"Ack");
                    if (theConnection != null) Log.Information($"Send {theConnection.ConnectionName} :{DateTime.UtcNow}");
                }
        }

        public void Update(object sender, UltrasonicDetectionEventArgs e)
        {
            _bus.Publish<IoTMessageReceived>(new IoTMessageReceived()
            {
                PayloadUltrasonic = e.Payload,
                Imei = e.Imei,
                CorrelationId = Guid.NewGuid(),
                Title = nameof(UltrasonicDetectionEventArgs),
                Timestamp = DateTime.UtcNow
            });

            Log.Information($"Update for UltrasonicDetectionEventArgs Before at :{DateTime.UtcNow}");
            Log.Information($"Count Connections at :{DateTime.UtcNow} {_theConnections?.Count}");
            if (_theConnections != null)
                foreach (var theConnection in _theConnections)
                {
                    theConnection?.Send(JsonConvert.SerializeObject(e.Payload.Last()));
                    if (theConnection != null) Log.Information($"Send {theConnection.ConnectionName} :{DateTime.UtcNow}");
                }

            Log.Information($"Update for UltrasonicDetectionEventArgs After at :{DateTime.UtcNow}");
        }
        public void Update(object sender, GPSDetectionEventArgs e)
        {
            _bus.Publish<IoTMessageReceived>(new IoTMessageReceived()
            {
                PayloadGps = e.Payload,
                Imei = e.Imei,
                CorrelationId = Guid.NewGuid(),
                Title = nameof(GPSDetectionEventArgs),
                Timestamp = DateTime.UtcNow
            });

            Log.Information($"Update for GPSDetectionEventArgs Before at :{DateTime.UtcNow}");
            Log.Information($"Count Connections at :{DateTime.UtcNow} {_theConnections?.Count}");
            if (_theConnections != null)
                foreach (var theConnection in _theConnections)
                {
                    theConnection?.Send(JsonConvert.SerializeObject(e.Payload));
                    if (theConnection != null) Log.Information($"Send {theConnection.ConnectionName} :{DateTime.UtcNow}");
                }

            Log.Information($"Update for GPSDetectionEventArgs After at :{DateTime.UtcNow}");
        }
        public void Update(object sender, DigitalDetectionEventArgs e)
        {
            _bus.Publish<IoTMessageReceived>(new IoTMessageReceived()
            {
                PayloadDigital = e.Payload,
                Imei = e.Imei,
                CorrelationId = Guid.NewGuid(),
                Title = nameof(DigitalDetectionEventArgs),
                Timestamp = DateTime.UtcNow
            });

            Log.Information($"Update for DigitalDetectionEventArgs Before at :{DateTime.UtcNow}");
            Log.Information($"Count Connections at :{DateTime.UtcNow} {_theConnections?.Count}");
            if (_theConnections != null)
                foreach (var theConnection in _theConnections)
                {
                    theConnection?.Send(JsonConvert.SerializeObject(e.Payload));
                    if (theConnection != null) Log.Information($"Send {theConnection.ConnectionName} :{DateTime.UtcNow}");
                }

            Log.Information($"Update for DigitalDetectionEventArgs After at :{DateTime.UtcNow}");
        }
    }
}