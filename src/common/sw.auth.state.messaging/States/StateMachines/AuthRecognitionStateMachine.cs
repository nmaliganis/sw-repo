using System;
using Automatonymous;
using sw.auth.state.messaging.Events.Impls;
using sw.auth.state.messaging.States.Commands.States.Closed;
using sw.auth.state.messaging.States.Commands.States.Finished;
using sw.auth.state.messaging.States.Commands.States.Processed;
using sw.auth.state.messaging.States.Commands.States.Registered;
using sw.auth.state.messaging.States.Commands.States.Rejected;
using sw.auth.state.messaging.States.Sagas;

namespace sw.auth.state.messaging.States.StateMachines;

public sealed class AuthRecognitionStateMachine : MassTransitStateMachine<AuthRecognitionSagaState>
{
  public State Registered { get; private set; }
  public State Processed { get; private set; }
  public State Finished { get; private set; }


  #region Commands

  public Event<IStartRegisterCommand> StartCommand { get; private set; }
  public Event<IProcessRegisterCommand> ProcessCommand { get; private set; }
  public Event<IRejectRegisterCommand> RejectCommand { get; private set; }
  public Event<IFinishRegisterCommand> FinishCommand { get; private set; }
  public Event<ICloseRegisterCommand> CloseCommand { get; private set; }

  #endregion

  public static AuthRecognitionStateMachine StateMachine { get; } = new AuthRecognitionStateMachine();

  private AuthRecognitionStateMachine()
  {
    InstanceState(s => s.CurrentState);

    Event(() => StartCommand,
      cc =>
        cc.CorrelateBy(state => state.Title, context =>
            context.Message.Title)
          .SelectId(context => Guid.NewGuid()));


    Event(() => StartCommand, x => x.CorrelateById(context =>
      context.Message.CorrelationId));
    Event(() => ProcessCommand, x => x.CorrelateById(context =>
      context.Message.CorrelationId));
    Event(() => RejectCommand, x => x.CorrelateById(context =>
      context.Message.CorrelationId));
    Event(() => FinishCommand, x => x.CorrelateById(context =>
      context.Message.CorrelationId));
    Event(() => CloseCommand, x => x.CorrelateById(context =>
      context.Message.CorrelationId));

    Initially(
      When(StartCommand)
        .Then(context =>
        {
          //log
          context.Instance.CorrelationId = context.Data.CorrelationId;
          context.Instance.ReceivedDateTime = DateTime.Now;
        })
        .ThenAsync(
          context => Console.Out.WriteLineAsync($"AuthRegistration" +
                                                $" {context.Data.MemberAuthId} received. Transition to Registered!"))
        //log
        .TransitionTo(Registered)
        .Publish(context => new RegisteringReceivedEvent(context.Instance))
    );

    During(Registered,
      When(ProcessCommand)
        .Then(context =>
        {
          //log
          context.Instance.CorrelationId = context.Data.CorrelationId;
          context.Instance.ReceivedDateTime = DateTime.Now;
        })
        .ThenAsync(
          context => Console.Out.WriteLineAsync(
            $"From Registered --> {context.Instance.Title} --->" +
            $"to Processed State"))
        .TransitionTo(Processed)
        .Publish(context => new ProcessingReceivedEvent(context.Instance)),
      When(RejectCommand)
        .Then(context => context.Instance.RejectedDateTime =
          DateTime.Now)
        .ThenAsync(
          //log
          context => Console.Out.WriteLineAsync(
            $"From Registered --> {context.Instance.Title} --->" +
            $"Finalized with Rejected status"))
        .Publish(context => new RejectingReceivedEvent(context.Instance))
        .Finalize()
    );

    During(Processed,
      When(ProcessCommand)
        .Then(context =>
        {
          Console.Out.WriteLineAsync($"Processing Recognition into Processed" +
                                     $" {context.Data.MemberStatus} received. Transition to Registered!");
          //log
          context.Instance.CorrelationId = context.Data.CorrelationId;
          context.Instance.ReceivedDateTime = DateTime.Now;
        })
        .ThenAsync(
          //log
          context => Console.Out.WriteLineAsync(
            $"From Processed --> {context.Instance.Title} --->" +
            $"to Processed State ---> with Precessed"))
        .TransitionTo(Processed)
        .Publish(context => new ProcessingReceivedEvent(context.Instance)),
      When(FinishCommand)
        .Then(context =>
        {
          context.Instance.CorrelationId = context.Data.CorrelationId;
          context.Instance.ReceivedDateTime = DateTime.Now;
        })
        .ThenAsync(
          context => Console.Out.WriteLineAsync(
            $"From Processed --> {context.Instance.Title} --->" +
            $"to Processed State ---> With Finished"))
        .TransitionTo(Finished)
        .Publish(context => new FinishingReceivedEvent(context.Instance)),
      When(RejectCommand)
        .Then(context => context.Instance.RejectedDateTime =
          DateTime.Now)
        .ThenAsync(
          context => Console.Out.WriteLineAsync(
            $"From Processed --> {context.Instance.Title} --->" +
            $"Finalized with Rejected status ---> With Rejected"))
        .Publish(context => new RejectingReceivedEvent(context.Instance))
        .Finalize()
    );
    During(Finished,
      When(ProcessCommand)
        .Then(context =>
        {
          Console.Out.WriteLineAsync($"Processing Recognition into Processed" +
                                     $" {context.Data.CorrelationId} received. Transition to Registered!");
        })
        .ThenAsync(
          context => Console.Out.WriteLineAsync(
            $"From Processed --> {context.Instance.Title} --->" +
            $"to Processed State ---> with Precessed"))
        .TransitionTo(Finished),
      When(CloseCommand)
        .Then(context =>
        {
          context.Instance.CorrelationId = context.Data.CorrelationId;
          context.Instance.ReceivedDateTime = DateTime.Now;
        })
        .ThenAsync(
          context => Console.Out.WriteLineAsync(
            $"From Finished --> {context.Instance.Title} --->" +
            $"Finalized with Close status ---> With Closed"))
        .Publish(context => new FinishingReceivedEvent(context.Instance))
        .Finalize()
    );

    SetCompletedWhenFinalized();
  }
}