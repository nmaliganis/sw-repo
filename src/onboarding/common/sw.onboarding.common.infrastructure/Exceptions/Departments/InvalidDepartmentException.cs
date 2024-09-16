using System;

namespace sw.auth.common.infrastructure.Exceptions.Departments
{
    public class InvalidDepartmentException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidDepartmentException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }//Class : InvalidDepartmentException

}//Namespace : sw.auth.common.infrastructure.Exceptions.Companies