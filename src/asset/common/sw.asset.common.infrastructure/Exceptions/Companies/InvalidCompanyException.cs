using System;

namespace sw.asset.common.infrastructure.Exceptions.Companies;

public class InvalidCompanyException : Exception
{
  public string BrokenRules { get; private set; }

  public InvalidCompanyException(string brokenRules)
  {
    BrokenRules = brokenRules;
  }
}//Class : InvalidCompanyException