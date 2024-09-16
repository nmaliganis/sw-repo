using System.Collections.Generic;
using System.Linq;

namespace sw.localization.api.Helpers.Models {
    /// <summary>
    /// Class : ErrorResponse
    /// </summary>
    public class ErrorResponse {
        /// <summary>
        /// Ctor : ErrorResponse
        /// </summary>
        public ErrorResponse() { }

        /// <summary>
        /// Ctor : ErrorResponse
        /// </summary>
        /// <param name="messages"></param>
        public ErrorResponse(IEnumerable<string> messages) {
            this.Messages = messages.ToArray();
        }

        /// <summary>
        /// Property : Messages
        /// </summary>
        public string[] Messages { get; set; }
    }
}
