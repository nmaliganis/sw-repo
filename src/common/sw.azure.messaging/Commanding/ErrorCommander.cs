namespace sw.azure.messaging.Commanding
{
    public sealed class ErrorCommander : BaseErrorCommander
    {
        public static ErrorCommander Instance { get; } = new ErrorCommander();
        #region Constructor
        /// <summary>
        /// Constructor : Commander
        /// </summary>
        private ErrorCommander()
        {
        }
        #endregion

    }//Class: ErrorCommander
}