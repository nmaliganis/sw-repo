using System;

namespace sw.auth.common.infrastructure.Exceptions.Departments {
    public class DepartmentAlreadyExistsException : Exception {
        public string Name { get; }
        public string BrokenRules { get; }

        public DepartmentAlreadyExistsException(string name, string brokenRules) {
            this.Name = name;
            this.BrokenRules = brokenRules;
        }

        public override string Message => $" Department with Name:{this.Name} already Exists!\n Additional info:{this.BrokenRules}";
    }//Class : DepartmentAlreadyExistsException

}//Namespace : sw.auth.common.infrastructure.Exceptions.Companies