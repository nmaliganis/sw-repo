using System;

namespace sw.asset.common.infrastructure.Exceptions.Simcards;

public class SimcardDoesNotExistException : Exception {
  public long SimcardId { get; }

  public SimcardDoesNotExistException(long simcardId) {
    this.SimcardId = simcardId;
  }

  public override string Message => $"Simcard with Id: {this.SimcardId}  doesn't exists!";
}//Class : SimcardDoesNotExistException