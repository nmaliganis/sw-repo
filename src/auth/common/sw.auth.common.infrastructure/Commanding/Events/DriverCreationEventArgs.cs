namespace sw.auth.common.infrastructure.Commanding.Events
{
    /// <summary>
    /// Class : DriverCreationEventArgs
    /// </summary>
    public class DriverCreationEventArgs : System.EventArgs
    {

        #region Properties - Variables (public)

        /// <summary>
        /// Property : Firstname
        /// </summary>
        public string Firstname { get; private set; }
        /// <summary>
        /// Property : Lastname
        /// </summary>
        public string Lastname { get; private set; }
        /// <summary>
        /// Property : Gender
        /// </summary>
        public string Gender { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Ctor for Member Registration Notification EventArgs
        /// </summary>
        public DriverCreationEventArgs(string firstname, string lastname, string gender)
        {
            Firstname = firstname;
            Lastname = lastname;
            Gender = gender;
        }

        #endregion

    }//Class : DriverCreationEventArgs
}