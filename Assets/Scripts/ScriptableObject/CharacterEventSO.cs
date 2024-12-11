using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRised;

    public void RaiseEvent(Character character)
    {
        OnEventRised?.Invoke(character);
    }
}
