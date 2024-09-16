using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;

namespace sw.azure.messaging.Commanding.Events
{

    /// <summary>
    /// Class EventArgs for IoT Message Notification Async 
    /// </summary>
    public class IoTDigitalMessageEventArgs : System.EventArgs
    {

        #region Properties - Variables (public)

        /// <summary>
        /// Property IsEnabled
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Property Imei
        /// </summary>
        public string Imei { get; private set; }

        /// <summary>
        /// Property Timestamp
        /// </summary>
        public DateTime Timestamp { get; private set; }


        /// <summary>
        /// Property Timestamp
        /// </summary>
        public List<IoTDigitalEvent> PayloadDigital { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Ctor for Member Registration Notification EventArgs
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadDigital"></param>
        public IoTDigitalMessageEventArgs(bool isEnabled, string imei, DateTime timestamp, List<IoTDigitalEvent> payloadDigital)
        {
            IsEnabled = isEnabled;
            Imei = imei;
            Timestamp = timestamp;
            PayloadDigital = payloadDigital;
        }

        #endregion

    }//Class: IoTDigitalMessageEventArgs
}