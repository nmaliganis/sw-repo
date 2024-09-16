using System;
using sw.auth.state.messaging.Events.Contracts;
using sw.auth.state.messaging.States.Sagas;

namespace sw.auth.state.messaging.Events.Impls
{
    public class FinishingReceivedEvent : IFinishingReceivedEvent
    {
        private readonly AuthRecognitionSagaState _authSagaState;

        public FinishingReceivedEvent(AuthRecognitionSagaState authSagaState)
        {
            this._authSagaState = authSagaState;
        }

        public Guid CorrelationId => _authSagaState.CorrelationId;
        public string Title => _authSagaState.Title;
    }
}