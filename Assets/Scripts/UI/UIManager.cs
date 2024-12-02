using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public CharacterEventSO characterEvent;
    public PlayerStatsBar playerStatsBar;

    private void OnEnable() {
        characterEvent.OnEventRised += CharacterChanged;
    }

    private void OnDisable() {
        characterEvent.OnEventRised -= CharacterChanged;
    }

    public void CharacterChanged(Character character)
    {
        float percentage = (float)character.currentHP/ character.maxHP;
        playerStatsBar?.OnHeartChange(percentage);
    }

}
