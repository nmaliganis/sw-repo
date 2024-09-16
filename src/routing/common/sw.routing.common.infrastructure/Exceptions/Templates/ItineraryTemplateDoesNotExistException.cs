using System;

namespace sw.routing.common.infrastructure.Exceptions.Templates
{
    public class ItineraryTemplateDoesNotExistException : Exception
    {
        public long ItineraryTemplateId { get; }

        public ItineraryTemplateDoesNotExistException(long itineraryTemplateId)
        {
            this.ItineraryTemplateId = itineraryTemplateId;
        }

        public override string Message => $"ItineraryTemplate with Id: {ItineraryTemplateId}  doesn't exists!";
    }
}