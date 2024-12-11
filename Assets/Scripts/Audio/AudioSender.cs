using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSender : MonoBehaviour
{

    public AudioEventSO audioEventSO;
    public AudioClip audioClip;

    public bool playOnEnable;

    private void OnEnable() {
        if (playOnEnable)
        {
            PlayAudioClip();
        }
    }

    public void PlayAudioClip() {
        audioEventSO?.OnEventRaised(audioClip);
    }
}
