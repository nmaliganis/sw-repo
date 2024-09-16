using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sw.routing.model.Itineraries;

internal enum ItineraryStatus
{
    Started = 1,
    Abandoned,
    Created,
    Canceled
}