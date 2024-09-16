using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;
using sw.interprocess.api.Commanding.Listeners.Inbounds;
using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;
using IoTUltrasonic = sw.azure.messaging.Models.IoT.IoTUltrasonic;

namespace sw.interprocess.api.Commanding.Servers.Base
{
    public abstract class WmInboundBaseServer
    {
        public event EventHandler<TelemetryRowDetectionEventArgs> TelemetryRowDetector;
        public event EventHandler<TelemetryDetectionEventArgs> TelemetryDetector;
        public event EventHandler<AttributeDetectionEventArgs> AttributeDetector;
        public event EventHandler<UltrasonicDetectionEventArgs> UltrasonicDetector;
        public event EventHandler<GPSDetectionEventArgs> GpsDetector;
        public event EventHandler<DigitalDetectionEventArgs> DigitalDetector;
        public event EventHandler<WsBroadcastMessageDetectionEventArgs> WsBroadcastMessageDetector;
        public event EventHandler<WsBroadcastAckMessageDetectionEventArgs> WsBroadcastAckMessageDetector;

        #region Telemetry detection Event Manipulation

        private void OnTelemetryDetection(TelemetryDetectionEventArgs e)
        {
            TelemetryDetector?.Invoke(this, e);
        }

        public void RaiseTelemetryDetection(string payload, string imei)
        {
            OnTelemetryDetection(new TelemetryDetectionEventArgs(payload, true, imei));
        }

        public void Attach(ITelemetryDetectionActionListener listener)
        {
            TelemetryDetector += listener.Update;
        }

        public void Detach(ITelemetryDetectionActionListener listener)
        {
            TelemetryDetector -= listener.Update;
        }

        #endregion

        #region Telemetry Row detection Event Manipulation

        private void OnTelemetryRowDetection(TelemetryRowDetectionEventArgs e)
        {
            TelemetryRowDetector?.Invoke(this, e);
        }

        public void RaiseTelemetryRowDetection(string payload, string imei)
        {
            OnTelemetryRowDetection(new TelemetryRowDetectionEventArgs(payload, true, imei));
        }

        public void Attach(ITelemetryRowDetectionActionListener listener)
        {
            TelemetryRowDetector += listener.Update;
        }

        public void Detach(ITelemetryRowDetectionActionListener listener)
        {
            TelemetryRowDetector -= listener.Update;
        }

        #endregion

        #region Attribute detection Event Manipulation

        private void OnAttributeDetection(AttributeDetectionEventArgs e)
        {
            AttributeDetector?.Invoke(this, e);
        }

        public void RaiseAttributeDetection(string payload, string imei)
        {
            OnAttributeDetection(new AttributeDetectionEventArgs(payload, true, imei));
        }

        public void Attach(IAttributeDetectionActionListener listener)
        {
            AttributeDetector += listener.Update;
        }

        public void Detach(IAttributeDetectionActionListener listener)
        {
            AttributeDetector -= listener.Update;
        }

        #endregion

        #region Ultrasonic detection Event Manipulation

        private void OnUltrasonicDetection(UltrasonicDetectionEventArgs e)
        {
            UltrasonicDetector?.Invoke(this, e);
        }

        public void RaiseUltrasonicDetection(List<IoTUltrasonic> payload, string imei)
        {
            OnUltrasonicDetection(new UltrasonicDetectionEventArgs(payload, true, imei));
        }

        public void Attach(IUltrasonicDetectionActionListener listener)
        {
            UltrasonicDetector += listener.Update;
        }

        public void Detach(IUltrasonicDetectionActionListener listener)
        {
            UltrasonicDetector -= listener.Update;
        }

        #endregion

        #region GPS detection Event Manipulation

        private void OnGPSDetection(GPSDetectionEventArgs e)
        {
            GpsDetector?.Invoke(this, e);
        }

        public void RaiseGPSDetection(IoTgps payload, string imei)
        {
            OnGPSDetection(new GPSDetectionEventArgs(payload, true, imei));
        }

        public void Attach(IGPSDetectionActionListener listener)
        {
            GpsDetector += listener.Update;
        }

        public void Detach(IGPSDetectionActionListener listener)
        {
            GpsDetector -= listener.Update;
        }

        #endregion

        #region Digital pins change status detection Event Manipulation

        private void OnDigitalDetection(DigitalDetectionEventArgs e)
        {
            DigitalDetector?.Invoke(this, e);
        }

        public void RaiseDigitalDetection(List<IoTDigitalEvent> payload, string imei)
        {
            OnDigitalDetection(new DigitalDetectionEventArgs(payload, true, imei));
        }

        public void Attach(IDigitalDetectionActionListener listener)
        {
            DigitalDetector += listener.Update;
        }

        public void Detach(IDigitalDetectionActionListener listener)
        {
            DigitalDetector -= listener.Update;
        }

        #endregion

        #region Ws Broadcast detection Event Manipulation

        private void OnWsBroadcastMessageDetection(WsBroadcastMessageDetectionEventArgs e)
        {
            WsBroadcastMessageDetector?.Invoke(this, e);
        }

        public void RaiseWsBroadcastMessageDetection(string payload)
        {
            OnWsBroadcastMessageDetection(new WsBroadcastMessageDetectionEventArgs(payload));
        }

        public void Attach(IWsBroadcastMessageDetectionActionListener listener)
        {
            WsBroadcastMessageDetector += listener.Update;
        }

        public void Detach(IWsBroadcastMessageDetectionActionListener listener)
        {
            WsBroadcastMessageDetector -= listener.Update;
        }

        #endregion

        #region Ws Broadcast Ack detection Event Manipulation

        private void OnWsBroadcastAckMessageDetection(WsBroadcastAckMessageDetectionEventArgs e)
        {
            WsBroadcastAckMessageDetector?.Invoke(this, e);
        }

        public void RaiseWsBroadcastAckMessageDetection(string payload)
        {
            OnWsBroadcastAckMessageDetection(new WsBroadcastAckMessageDetectionEventArgs(payload));
        }

        public void Attach(IWsBroadcastAckMessageDetectionActionListener listener)
        {
            WsBroadcastAckMessageDetector += listener.Update;
        }

        public void Detach(IWsBroadcastAckMessageDetectionActionListener listener)
        {
            WsBroadcastAckMessageDetector -= listener.Update;
        }

        #endregion
    }
}