using System;

namespace sw.auth.common.infrastructure.Exceptions.Companies
{
    public class InvalidCompanyException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidCompanyException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }//Class : InvalidCompanyException
}//Namespace : sw.auth.common.infrastructure.Exceptions.Companies