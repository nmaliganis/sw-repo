using System;

namespace sw.auth.common.infrastructure.Exceptions.Departments {
    public class DepartmentDoesNotExistException : Exception {
        public long DepartmentId { get; }

        public DepartmentDoesNotExistException(long departmentId) {
            this.DepartmentId = departmentId;
        }

        public override string Message => $"Department with Id: {this.DepartmentId}  doesn't exists!";
    }//Class : DepartmentDoesNotExistException

}//Namespace : sw.auth.common.infrastructure.Exceptions.Companies