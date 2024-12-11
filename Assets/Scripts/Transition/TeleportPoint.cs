using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInterctable
{
    public SceneLoadEventSO sceneLoader;
    public Vector3 positionToGo;
    public GameSceneSO nextScene;
    public bool fadeScreen;

    public void TakeAction()
    {
        Debug.Log("Teleport");
        sceneLoader.RaiseEvent(nextScene, positionToGo, fadeScreen);
    }
    // Target scene

}
