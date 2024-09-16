using System;
using sw.auth.state.messaging.Events.Contracts;
using sw.auth.state.messaging.States.Sagas;

namespace sw.auth.state.messaging.Events.Impls
{
    public class RegisteringReceivedEvent : IRegisteringReceivedEvent
    {
        private readonly AuthRecognitionSagaState _authSagaState;

        public RegisteringReceivedEvent(AuthRecognitionSagaState authSagaState)
        {
            this._authSagaState = authSagaState;
        }

        public Guid CorrelationId => _authSagaState.CorrelationId;
        public string Title => _authSagaState.Title;
    }

    public class RejectingReceivedEvent : IRejectingReceivedEvent
    {
        private readonly AuthRecognitionSagaState _authSagaState;

        public RejectingReceivedEvent(AuthRecognitionSagaState authSagaState)
        {
            this._authSagaState = authSagaState;
        }

        public Guid CorrelationId => _authSagaState.CorrelationId;
        public string ErrorMessage => _authSagaState.ErrorMessage;
    }
}