using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace sw.logging.api.Configurations.Installers;

/// <summary>
/// Class : ProtectedSettersContractResolver
/// </summary>
public class ProtectedSettersContractResolver : DefaultContractResolver
{
    /// <summary>
    /// Method : CreateProperty
    /// </summary>
    /// <param name="member"></param>
    /// <param name="memberSerialization"></param>
    /// <returns></returns>
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);

        if (!prop.Writable)
        {
            var property = member as PropertyInfo;
            if (property != null)
            {
                var hasSetter = property.GetSetMethod(true) != null;
                prop.Writable = hasSetter;
            }
        }
        return prop;
    }
}