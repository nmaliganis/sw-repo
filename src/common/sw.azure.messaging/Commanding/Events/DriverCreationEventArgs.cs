namespace sw.azure.messaging.Commanding.Events
{
    /// <summary>
    /// Class : DriverCreationEventArgs
    /// </summary>
    public class DriverCreationEventArgs : System.EventArgs
    {

        #region Properties - Variables (public)

        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public string Gender { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Ctor for Member Registration Notification EventArgs
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadGps"></param>
        public DriverCreationEventArgs(string firstname, string lastname, string gender)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Gender = gender;
        }

        #endregion

    }//Class : DriverCreationEventArgs
}