using Newtonsoft.Json;

namespace sw.infrastructure.Serializers
{
  public class JsonSerializer : IJsonSerializer
  {
    public T DeserializeObject<T>(string json)
    {
      return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
      {
        TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
      });
    }

    public string SerializeObject(object item)
    {
      return JsonConvert.SerializeObject(item, new JsonSerializerSettings { Formatting = Formatting.None });
    }
  }
}