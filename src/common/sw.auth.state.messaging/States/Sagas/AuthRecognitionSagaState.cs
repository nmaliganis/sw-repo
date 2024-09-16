using System;
using Automatonymous;

namespace sw.auth.state.messaging.States.Sagas;

public class AuthRecognitionSagaState : SagaStateMachineInstance
{
  public Guid CorrelationId { get; set; }
  public State CurrentState { get; set; }

  public string ErrorMessage { get; set; }
  public DateTime ReceivedDateTime { get; set; }
  public DateTime RejectedDateTime { get; set; }
  public long MemberAuthId { get; set; }
  public string Title { get; set; }
}