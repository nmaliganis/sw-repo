using System;

namespace sw.asset.common.infrastructure.Exceptions.Assets.Containers;

public class ContainerDoesNotExistAfterMadePersistentException : Exception
{
    public string Name { get; private set; }
    public string CompanyName { get; }

    public ContainerDoesNotExistAfterMadePersistentException(string name)
    {
        Name = name;
    }

    public ContainerDoesNotExistAfterMadePersistentException(string name, string companyName)
    {
        Name = name;
        CompanyName = companyName;
    }

    public override string Message => $" Container with Name: {Name} for Company with Name : {CompanyName} was not made Persistent!";

}// Class : ContainerDoesNotExistAfterMadePersistentException