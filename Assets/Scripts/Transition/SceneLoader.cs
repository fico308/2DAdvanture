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
    public Transform playerTransform;
    public Vector3 playerPosition;
    public float fadeDuration;
    public GameSceneSO firstScene;

    [Header("Event Listener")]
    public SceneLoadEventSO sceneLoader;

    [Header("Event Broadcaster")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeImageEventSO fadeImageEvent;
    public SceneLoadEventSO newSceneLoadEvent;


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
        // 必须这样

        NewGame();
    }

    private void NewGame()
    {
        // FIX: fade必须为true否则黑屏
        sceneLoader.RaiseEvent(firstScene, playerPosition, true);
        // OnLoadSceneRequestEvent(firstScene, playerPosition, true);
    }

    private void OnEnable()
    {
        sceneLoader.OnEventRaised += OnLoadSceneRequestEvent;
    }

    private void OnDisable()
    {
        sceneLoader.OnEventRaised -= OnLoadSceneRequestEvent;
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
        Debug.Log("load new " + newScene.scene + "-"+posToGo.x + "-"+posToGo.y);
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
