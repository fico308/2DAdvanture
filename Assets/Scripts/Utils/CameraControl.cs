using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [Header("Event Listerner")]
    public VoidEventSO afterSceneLoadedEvent;

    private CinemachineConfiner2D confiner;

    private void Awake() {
        confiner = GetComponent<CinemachineConfiner2D>();
    }


    private void OnEnable() {
        afterSceneLoadedEvent.OnEventRaised += NewSceneLoaded;
    }

    private void OnDisable() {
        afterSceneLoadedEvent.OnEventRaised -= NewSceneLoaded;
    }

    private void NewSceneLoaded()
    {
        GetNewGamerBounds();
    }

    private void GetNewGamerBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null)
        {
            return;
        }
        confiner.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner.InvalidateCache();
    }
}
