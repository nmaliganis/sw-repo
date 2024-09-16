using System;

namespace sw.asset.common.infrastructure.Exceptions.Simcards;

public class SimcardAlreadyExistsException : Exception {
  public string Name { get; }
  public string BrokenRules { get; }

  public SimcardAlreadyExistsException(string name, string brokenRules) {
    this.Name = name;
    this.BrokenRules = brokenRules;
  }

  public override string Message => $" Simcard with Name:{this.Name} already Exists!\n Additional info:{this.BrokenRules}";
}//Class : SimcardAlreadyExistsException