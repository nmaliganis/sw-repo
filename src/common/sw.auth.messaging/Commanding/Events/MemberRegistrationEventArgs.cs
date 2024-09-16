namespace sw.auth.messaging.Commanding.Events
{

    /// <summary>
    /// Class EventArgs for Member Registration Notification Async 
    /// </summary>
    public class MemberRegistrationEventArgs : System.EventArgs
    {

        #region Properties - Variables (public)

        /// <summary>
        /// Property IsEnabled
        /// </summary>
        public bool IsEnabled { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Ctor for Member Registration Notification EventArgs
        /// </summary>
        /// <param name="isEnabled"></param>
        public MemberRegistrationEventArgs(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        #endregion

    }//Class: MemberRegistrationEventArgs

}// Namespace : sw.auth.messaging.Commanding.Events