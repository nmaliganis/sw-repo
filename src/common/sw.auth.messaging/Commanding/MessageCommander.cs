namespace sw.auth.messaging.Commanding
{

    /// <summary>
    /// Class MessageCommander
    /// </summary>
    public sealed class MessageCommander : BaseMessageCommander
    {

        /// <summary>
        /// Instance of MessageCommander 
        /// </summary>
        public static readonly MessageCommander Instance = new();

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private MessageCommander()
        {
        }
        #endregion

    }//Class: MessageCommander

}// Namespace : sw.auth.messaging.Commanding