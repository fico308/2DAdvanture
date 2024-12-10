using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource sfxAudio;
    public AudioSource bgmAudio;

    public AudioEventSO audioEventSO;

    private void OnEnable() {
        audioEventSO.OnEventRaised += PlayAudio;
    }


    private void OnDisable() {
        audioEventSO.OnEventRaised -= PlayAudio;
    }


    private void PlayAudio(AudioClip audioClip)
    {
        sfxAudio.clip = audioClip;
        sfxAudio.Play();
    }
}
