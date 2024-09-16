using System;

namespace sw.asset.common.infrastructure.Exceptions.Companies
{
    public class CompanyDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public CompanyDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" Company with Name: {Name} was not made Persistent!";

    } // Class : CompanyDoesNotExistAfterMadePersistentException
}