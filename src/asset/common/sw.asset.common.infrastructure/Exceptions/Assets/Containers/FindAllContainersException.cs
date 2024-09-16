using System;

namespace sw.asset.common.infrastructure.Exceptions.Assets.Containers;

public class FindAllContainersException : Exception
{
  private readonly string _messageDetails;

  public FindAllContainersException(string messageDetails)
  {
    this._messageDetails = messageDetails;
  }

  public override string Message => $"Find all Containers error: {_messageDetails}";
}