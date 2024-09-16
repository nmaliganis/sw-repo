using System;

namespace sw.asset.common.infrastructure.Exceptions.Assets.Containers;

public class ContainerDoesNotExistException : Exception {
  public long ContainerId { get; }

  public ContainerDoesNotExistException(long containerId) {
    this.ContainerId = containerId;
  }

  public override string Message => $"Container with Id: {this.ContainerId}  doesn't exists!";
}//Class : ContainerDoesNotExistException