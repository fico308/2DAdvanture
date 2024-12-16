using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using YamlDotNet.Serialization;

/// <summary>
/// 暂存需要保存的数据到内存中
/// </summary>
[Serializable]
public class SaveData
{
    public Dictionary<string, CharacterData> characters = new Dictionary<string, CharacterData>();
    public string currentScene;

    [Serializable]
    public struct CharacterData
    {
        public string ID;
        public int hp;
        public float positionX;
        public float positionY;
        public float positionZ;
    }

    public void SaveScene(GameSceneSO gameScene)
    {
        currentScene = JsonUtility.ToJson(gameScene);
    }

    public GameSceneSO LoadScene()
    {
        GameSceneSO gameScene = new GameSceneSO();
        JsonUtility.FromJsonOverwrite(currentScene, gameScene);
        return gameScene;
    }

}