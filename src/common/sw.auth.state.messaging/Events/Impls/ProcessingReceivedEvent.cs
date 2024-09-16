using System;
using sw.auth.state.messaging.Events.Contracts;
using sw.auth.state.messaging.States.Sagas;

namespace sw.auth.state.messaging.Events.Impls
{
    public class ProcessingReceivedEvent : IProcessingReceivedEvent
    {
        private readonly AuthRecognitionSagaState _authSagaState;

        public ProcessingReceivedEvent(AuthRecognitionSagaState authSagaState)
        {
            this._authSagaState = authSagaState;
        }

        public Guid CorrelationId => _authSagaState.CorrelationId;
        public string Title => _authSagaState.Title;
    }
}