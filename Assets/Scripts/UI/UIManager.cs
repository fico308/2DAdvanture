using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Game object")]
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject mobileButton;
    public AudioMixer audioMixer;
    public Button settingBtn;
    public GameObject settingPanel;
    public Slider slider;

    [Header("Event Listener")]
    public CharacterEventSO characterEvent;
    public PlayerStatsBar playerStatsBar;
    public SceneLoadEventSO newSceneLoadEvent;
    public VoidEventSO gameOverEvent;

    public Button.ButtonClickedEvent PauseGame { get; private set; }

    private void Awake()
    {
#if UNITY_STANDALONE
        mobileButton.SetActive(false);
#else
        mobileButton.SetActive(true);
#endif
    }

    private void OnEnable()
    {
        characterEvent.OnEventRised += CharacterChanged;
        newSceneLoadEvent.OnEventRaised += NewSceneLoad;
        gameOverEvent.OnEventRaised += GameOver;
        settingBtn.onClick.AddListener(ToggleSettingButton);
    }

    private void OnDisable()
    {
        characterEvent.OnEventRised -= CharacterChanged;
        newSceneLoadEvent.OnEventRaised -= NewSceneLoad;
        gameOverEvent.OnEventRaised -= GameOver;
        settingBtn.onClick.RemoveListener(ToggleSettingButton);
    }

    private void ToggleSettingButton()
    {
        if (settingPanel.activeSelf)
        {
            // stop pause
            settingPanel.SetActive(false);
            Time.timeScale = 1;
        } else {
            // pause game
            settingPanel.SetActive(true);
            Time.timeScale = 0;
            FillVolume();
        }
    }

    private void FillVolume()
    {
        float volume;
        audioMixer.GetFloat(Constants.MasterVolumeName, out volume);
        slider.value = (volume+80)/100;
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
