using System;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

/// <summary>
/// 暂存需要保存的数据到内存中
/// </summary>
[Serializable]
public class SaveData
{
    public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();

    [Serializable]
    public struct CharacterData
    {
        public string ID;
        public int hp;
        public float positionX;
        public float positionY;
        public float positionZ;
    }

    // public static string ToYaml(SaveData saveData)
    // {
    //     var serializer = new SerializerBuilder().Build();
    //     return serializer.Serialize(saveData);
    // }

    // public static SaveData LoadFromYaml(string yaml)
    // {
    //     var deserializer = new DeserializerBuilder().Build();
    //     return deserializer.Deserialize<SaveData>(yaml);
    // }

    // [Obsolete("Use ToYaml instead")]
    // public string ToJson()
    // {
    //     return JsonUtility.ToJson(this);
    // }

    // [Obsolete("Use LoadFromYaml instaed")]
    // public void LoadFromJson(string json)
    // {
    //     JsonUtility.FromJsonOverwrite(json, this);
    // }

}