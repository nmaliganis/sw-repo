using System;

namespace sw.infrastructure.Exceptions.Common
{
    public class DomainException : Exception
    {
        private string _notApplicableMsg;

        public DomainException(string notApplicableMsg)
        {
            this._notApplicableMsg = notApplicableMsg;
        }
    }
}