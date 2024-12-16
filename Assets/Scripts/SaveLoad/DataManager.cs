using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: https://github.com/UnityTechnologies/UniteNow20-Persistent-Data/blob/main/SaveDataManager.cs
public class DataManager : MonoBehaviour
{
    const string DataFileLocation = "data.dat";

    [Header("Event listener")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    public static DataManager instance;

    // TODO: test only
    public VoidEventSO newGameEvent;

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
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
        newGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        Debug.Log("DataManager NewGame");
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
            Debug.Log($"Read JSON: {json}");
            SaveData saveData = SerializeUtil.LoadFromYaml<SaveData>(json);

            foreach (var saveable in saveables)
            {
                saveable.Load(saveData);
            }
            Debug.Log("Load complete");
        }
    }

}
