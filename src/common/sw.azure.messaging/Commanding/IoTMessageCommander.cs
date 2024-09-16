namespace sw.azure.messaging.Commanding
{

    /// <summary>
    /// Class IoTMessageCommander
    /// </summary>
    public sealed class IoTMessageCommander : BaseIoTMessageCommander
    {

        /// <summary>
        /// Instance of IoTMessageCommander 
        /// </summary>
        public static IoTMessageCommander Instance { get; } = new IoTMessageCommander();

        #region Constructor
        /// <summary>
        /// Constructor : IoTMessageCommander
        /// </summary>
        private IoTMessageCommander()
        {
        }
        #endregion

    }//Class: IoTMessageCommander
}