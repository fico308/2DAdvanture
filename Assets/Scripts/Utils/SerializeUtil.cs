using YamlDotNet.Serialization;

public class SerializeUtil
{
    public static string ToYaml(object obj)
    {
        var serializer = new SerializerBuilder().Build();
        return serializer.Serialize(obj);
    }

    public static T LoadFromYaml<T>(string yaml)
    {
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<T>(yaml);
    }
}