using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameSceneSO firstScene;
    public SceneLoadEventSO sceneLoader;

    private GameSceneSO currentScene;

    private void Awake() {
        // 加载scene
        // 由于浅拷贝的问题, 这里不能这样load
        // Addressables.LoadSceneAsync(firstScene.scene, LoadSceneMode.Additive);
        // 必须这样
        firstScene.scene.LoadSceneAsync(LoadSceneMode.Additive);
        currentScene = firstScene;
    }

    private void OnEnable() {
        sceneLoader.OnEventRaised += LoadNewScene;
    }

    private void OnDisable() {
        sceneLoader.OnEventRaised -= LoadNewScene;
    }


    private void LoadNewScene(GameSceneSO newScene, Vector3 posToGo, bool fadeScreen)
    {
        Debug.Log("load new");
        // 卸载旧的
        currentScene.scene.UnLoadScene();

        // 加载新的
        newScene.scene.LoadSceneAsync(LoadSceneMode.Additive);
        currentScene = newScene;
    }

}
