using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject {
    public UnityAction<GameSceneSO, Vector3, bool> OnEventRaised;

    /// <summary>
    /// 传递要跳转的场景
    /// </summary>
    /// <param name="gameScene">新场景</param>
    /// <param name="posToGo">初始位置</param>
    /// <param name="fadeScreen">是否黑屏加载</param>
    public void RaiseEvent(GameSceneSO gameScene, Vector2 posToGo, bool fadeScreen)
    {
        OnEventRaised?.Invoke(gameScene, posToGo, fadeScreen);
    }
}