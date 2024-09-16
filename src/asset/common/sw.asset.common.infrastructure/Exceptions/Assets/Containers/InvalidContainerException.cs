using System;

namespace sw.asset.common.infrastructure.Exceptions.Assets.Containers;

public class InvalidContainerException : Exception
{
  public string BrokenRules { get; private set; }

  public InvalidContainerException(string brokenRules)
  {
    BrokenRules = brokenRules;
  }
}//Class : InvalidContainerException