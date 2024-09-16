using System;
using System.Collections.Generic;

namespace sw.interprocess.api.Models.Requests
{
    public class WsPostMulticastRequestDto
    {
        public List<Guid> Recipient { get; set; }
        public string Message { get; set; }
    }
}