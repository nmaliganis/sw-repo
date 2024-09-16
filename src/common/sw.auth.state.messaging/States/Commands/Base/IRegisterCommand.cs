using System;

namespace sw.auth.state.messaging.States.Commands.Base;

public interface IRegisterCommand
{
  Guid CorrelationId { get; set; }
  DateTime Timestamp { get; set; }
}