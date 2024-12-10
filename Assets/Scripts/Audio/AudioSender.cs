using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSender : MonoBehaviour
{

    public AudioEventSO audioEventSO;
    public AudioClip audioClip;

    private void OnEnable() {
        audioEventSO?.OnEventRaised(audioClip);
    }
}
