using System;

namespace sw.asset.common.infrastructure.Exceptions.Companies {
    public class CompanyAlreadyExistsException : Exception {
        public string Name { get; }
        public string BrokenRules { get; }

        public CompanyAlreadyExistsException(string name, string brokenRules) {
            this.Name = name;
            this.BrokenRules = brokenRules;
        }

        public override string Message => $" Company with Name:{this.Name} already Exists!\n Additional info:{this.BrokenRules}";
    }//Class : CompanyAlreadyExistsException
}//Namespace : sw.auth.common.infrastructure.Exceptions.Companies