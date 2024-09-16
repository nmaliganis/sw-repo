using System;

namespace sw.auth.common.infrastructure.Exceptions.Departments
{
    public class DepartmentDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public DepartmentDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }
        public override string Message => $" Department with Name: {Name} was not made Persistent!";

    }// Class : DepartmentDoesNotExistAfterMadePersistentException

}//Namespace : sw.auth.common.infrastructure.Exceptions.Companies