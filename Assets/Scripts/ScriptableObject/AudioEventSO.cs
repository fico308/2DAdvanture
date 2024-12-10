using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/AudioEventSO")]
public class AudioEventSO : ScriptableObject
{
    public UnityAction<AudioClip> OnEventRaised;

    public void RaiseEvent(AudioClip audioClip)
    {
        OnEventRaised?.Invoke(audioClip);
    }

}
