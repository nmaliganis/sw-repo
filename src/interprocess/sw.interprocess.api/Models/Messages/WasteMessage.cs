using sw.interprocess.api.Models.Messages.Base;
using System.Collections.Generic;

namespace sw.interprocess.api.Models.Messages
{
  public class WasteMessage : BaseMessage
  {
    public List<string> Commands { get; set; } = new List<string>();
  }
}