using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public AudioMixer mixer;

    [Header("AudioSource")]
    public AudioSource sfxAudio;
    public AudioSource bgmAudio;

    [Header("Event listener")]
    public AudioEventSO audioEventSO;
    public FloatEventSO volumeUpdatedEvent;

    private void OnEnable() {
        audioEventSO.OnEventRaised += PlayAudio;
        volumeUpdatedEvent.OnEventRaised += UpdateVolume;
    }


    private void OnDisable() {
        audioEventSO.OnEventRaised -= PlayAudio;
        volumeUpdatedEvent.OnEventRaised -= UpdateVolume;
    }

    private void UpdateVolume(float newVolume)
    {
        mixer.SetFloat(Constants.MasterVolumeName, newVolume * 100 - 80); // volume range: [-80 20]
    }

    private void PlayAudio(AudioClip audioClip)
    {
        sfxAudio.clip = audioClip;
        sfxAudio.Play();
    }
}
