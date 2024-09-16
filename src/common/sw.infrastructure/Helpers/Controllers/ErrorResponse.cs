using System.Collections.Generic;
using System.Linq;

namespace sw.infrastructure.Helpers.Controllers
{
    internal class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(IEnumerable<string> messages) => Messages = messages.ToArray();

        public string[] Messages { get; set; }
    }
}