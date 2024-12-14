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
    public SceneLoadEventSO sceneLoader;
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
        sceneLoader.RaiseEvent(menuScene, menuPlayerPosition, true);
    }



    private void OnEnable()
    {
        sceneLoader.OnEventRaised += OnLoadSceneRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        sceneLoader.OnEventRaised -= OnLoadSceneRequestEvent;
    }


    private void NewGame()
    {
        // 加载游戏场景
        sceneLoader.RaiseEvent(gameScene, gamePlayerPosition, true);
    }


    private void OnLoadSceneRequestEvent(GameSceneSO newScene, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        fadeScene = fadeScreen;
        sceneToGo = newScene;
        this.posToGo = posToGo;
        Debug.Log("load new " + newScene.scene + "-" + posToGo.x + "-" + posToGo.y);
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
        Debug.Log("unload1 " + currentScene);
        if (currentScene != null)
        {
            Debug.Log("unload2 " + currentScene.scene);
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
        Debug.Log("scene loaded");
        // 通知场景加载成功
        isLoading = false;
    }
}
