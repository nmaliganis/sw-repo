using System;

namespace sw.asset.common.infrastructure.Exceptions.Simcards;

public class SimcardDoesNotExistAfterMadePersistentException : Exception
{
  public string Name { get; private set; }

  public SimcardDoesNotExistAfterMadePersistentException(string name)
  {
    Name = name;
  }

  public override string Message => $" Simcard with Name: {Name} was not made Persistent!";
}// Class : SimcardDoesNotExistAfterMadePersistentException