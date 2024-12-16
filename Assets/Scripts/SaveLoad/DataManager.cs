using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: https://github.com/UnityTechnologies/UniteNow20-Persistent-Data/blob/main/SaveDataManager.cs
public class DataManager : MonoBehaviour
{
    // TODO: support multi save files
    private const string DataFileLocation = "data.dat";

    [Header("Variables")]
    public SceneLoader sceneLoader;

    [Header("Event listener")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    public static DataManager instance;

    private List<ISaveable> saveables = new List<ISaveable>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }


    public void RegisterSaveData(ISaveable saveable)
    {
        if (saveables.Contains(saveable))
        {
            return;
        }
        saveables.Add(saveable);
    }

    public void UnregisterSaveData(ISaveable saveable)
    {
        if (!saveables.Contains(saveable))
        {
            return;
        }
        saveables.Remove(saveable);
    }

    public void Save()
    {
        SaveData saveData = new SaveData();
        foreach (var saveable in saveables)
        {
            saveable.Save(saveData);
        }
        foreach (var item in saveData.characters)
        {
            Debug.Log($"Save\t{item.Key}\t{item.Value.hp}\t{item.Value.positionX}\t{item.Value.positionY}\t{item.Value.positionZ}");
        }
        // Save scene
        sceneLoader.SaveScene(saveData);

        // Save to file
        if (!FileManager.WriteToFile(DataFileLocation, SerializeUtil.ToYaml(saveData)))
        {
            Debug.LogError("Save file fail");
        }
    }

    public void Load()
    {
        if (FileManager.LoadFromFile(DataFileLocation, out var json))
        {
            Debug.Log($"Read YAML: {json}");
            SaveData saveData = SerializeUtil.LoadFromYaml<SaveData>(json);
            // load scene first
            // FIXME: 场景加载使用event, 会有延时, 因此player会漂浮在空中一段时间
            sceneLoader.LoadScene(saveData);

            foreach (var saveable in saveables)
            {
                saveable.Load(saveData);
            }
            Debug.Log("Load complete");
        }
    }

}
