using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Base Variables")]
    public Transform playerTransform;
    public float fadeDuration;

    [Header("Menu")]
    public GameSceneSO menuScene;
    public Vector3 menuPlayerPosition;
    [Header("Game")]
    public GameSceneSO gameScene;
    public Vector3 gamePlayerPosition;

    [Header("Event Listener")]
    public SceneLoadEventSO sceneLoaderEvent;
    public VoidEventSO newGameEvent;

    [Header("Event Broadcaster")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeImageEventSO fadeImageEvent;
    public SceneLoadEventSO scendUnloadedEvent;

    private GameSceneSO sceneToGo;
    private Vector3 posToGo;
    private GameSceneSO currentScene;
    private bool fadeScene;
    private bool isLoading;

    private void Start()
    {
        // 加载scene
        // 由于浅拷贝的问题, 这里不能这样load
        // Addressables.LoadSceneAsync(firstScene.scene, LoadSceneMode.Additive);


        // 游戏开始, 加载menu场景
        LoadMenu();
    }

    private void OnEnable()
    {
        sceneLoaderEvent.OnEventRaised += OnLoadSceneRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        sceneLoaderEvent.OnEventRaised -= OnLoadSceneRequestEvent;
    }

    public void LoadMenu()
    {
        sceneLoaderEvent.RaiseEvent(menuScene, menuPlayerPosition, true);
    }

    private void NewGame()
    {
        // 加载游戏场景
        sceneLoaderEvent.RaiseEvent(gameScene, gamePlayerPosition, true);
    }


    private void OnLoadSceneRequestEvent(GameSceneSO newScene, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        Debug.Log($"Load new scene {newScene}");

        fadeScene = fadeScreen;
        sceneToGo = newScene;
        this.posToGo = posToGo;
        if (currentScene != null)
        {
            StartCoroutine(UnLoadOldAndLoadNew());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadOldAndLoadNew()
    {
        if (fadeScene)
        {
            // 淡入
            fadeImageEvent.FaedIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);
        if (currentScene != null)
        {
            scendUnloadedEvent.RaiseEvent(currentScene, posToGo, false);
            yield return currentScene.scene.UnLoadScene();
        }

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingState = sceneToGo.scene.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingState.Completed += LoadingComplete;
    }

    private void LoadingComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        // 加载成功
        currentScene = sceneToGo;
        // 调整人物坐标
        playerTransform.position = posToGo;
        // 淡入淡出
        if (fadeScene)
        {
            // 淡出
            fadeImageEvent.FaedOut(fadeDuration);
        }
        afterSceneLoadedEvent?.RaiseEvent();
        // 通知场景加载成功
        isLoading = false;
    }

    public void SaveScene(SaveData saveData)
    {
        saveData.SaveScene(currentScene);
    }

    public void LoadScene(SaveData saveData)
    {
        var playerId = playerTransform.GetComponent<DataDefination>()?.ID;
        if (saveData.characters.TryGetValue(playerId, out var cd))
        {
            sceneLoaderEvent.RaiseEvent(saveData.LoadScene(), new Vector3(cd.positionX, cd.positionY, cd.positionZ), fadeScene);
        }
    }
}
