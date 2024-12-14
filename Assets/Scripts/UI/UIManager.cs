using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Event Listener")]
    public CharacterEventSO characterEvent;
    public PlayerStatsBar playerStatsBar;
    public SceneLoadEventSO newSceneLoadEvent;

    private void OnEnable()
    {
        characterEvent.OnEventRised += CharacterChanged;
        newSceneLoadEvent.OnEventRaised += NewSceneLoad;
    }

    private void OnDisable()
    {
        characterEvent.OnEventRised -= CharacterChanged;
        newSceneLoadEvent.OnEventRaised -= NewSceneLoad;
    }

    private void NewSceneLoad(GameSceneSO newScene, Vector3 pos, bool faedIn)
    {
        playerStatsBar.gameObject.SetActive(newScene.type == SceneType.Menu);
    }

    public void CharacterChanged(Character character)
    {
        float percentage = (float)character.currentHP / character.maxHP;
        playerStatsBar?.OnHeartChange(percentage);
    }

}
