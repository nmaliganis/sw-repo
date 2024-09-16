using sw.routing.model.ItineraryTemplates;
using NHibernate.Type;

namespace sw.routing.model.CustomTypes;

public class StreamStringType : EnumStringType
{
    public StreamStringType() : base(typeof(StreamType))
    {
    }
}