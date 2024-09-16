using System;

namespace sw.asset.common.infrastructure.Exceptions.Assets.Containers;

public class ContainerAlreadyExistsException : Exception
{
    public string Name { get; }
    public string CompanyName { get; }
    public string BrokenRules { get; }

    public ContainerAlreadyExistsException(string name, string brokenRules)
    {
        Name = name;
        BrokenRules = brokenRules;
    }

    public ContainerAlreadyExistsException(string name, string companyName, string brokenRules)
    {
        Name = name;
        CompanyName = companyName;
        BrokenRules = brokenRules;
    }

    public override string Message => $" Container with Name:{Name} for Company with Name : {CompanyName} already Exists!\n Additional info:{BrokenRules}";
}//Class : ContainerAlreadyExistsException