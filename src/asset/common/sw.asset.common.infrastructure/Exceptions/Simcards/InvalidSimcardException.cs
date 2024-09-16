using System;

namespace sw.asset.common.infrastructure.Exceptions.Simcards;

public class InvalidSimcardException : Exception
{
  public string BrokenRules { get; private set; }

  public InvalidSimcardException(string brokenRules)
  {
    BrokenRules = brokenRules;
  }
}//Class : InvalidSimcardException