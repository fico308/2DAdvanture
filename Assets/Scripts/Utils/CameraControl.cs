using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner;

    private void Awake() {
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    // Start is called before the first frame update
    void Start()
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
