using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Game object")]
    public GameObject gameOverPanel;
    public GameObject restartButton;

    [Header("Event Listener")]
    public CharacterEventSO characterEvent;
    public PlayerStatsBar playerStatsBar;
    public SceneLoadEventSO newSceneLoadEvent;
    public VoidEventSO gameOverEvent;

    private void OnEnable()
    {
        characterEvent.OnEventRised += CharacterChanged;
        newSceneLoadEvent.OnEventRaised += NewSceneLoad;
        gameOverEvent.OnEventRaised += GameOver;
    }

    private void OnDisable()
    {
        characterEvent.OnEventRised -= CharacterChanged;
        newSceneLoadEvent.OnEventRaised -= NewSceneLoad;
        gameOverEvent.OnEventRaised -= GameOver;
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
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
