namespace sw.azure.messaging.Commanding
{

    /// <summary>
    /// Class IoTMessageCommander
    /// </summary>
    public sealed class Commander : BaseCommander
    {

        /// <summary>
        /// Instance of Commander 
        /// </summary>
        public static Commander Instance { get; } = new Commander();
        #region Constructor
        /// <summary>
        /// Constructor : Commander
        /// </summary>
        private Commander()
        {
        }
        #endregion

    }//Class: Commander
}