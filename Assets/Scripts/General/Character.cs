using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("Base Variables")]
    public int maxHP;
    public int currentHP;
    public bool isDead;
    public bool dieInWater;


    [Header("Invulnerable")]
    public float invulnerableDuration;
    public float invulnerableCounter;
    public bool isInvulnerable;

    // Events
    [Header("Event broadcaster")]
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;
    public UnityEvent<Character> OnCharacterChange;
    [Header("Event listener")]
    public VoidEventSO newGameEvent;

    private void OnEnable() {
        newGameEvent.OnEventRaised += NewGame;
    }

    private void OnDisable() {
        newGameEvent.OnEventRaised -= NewGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInvulnerable)
        {
            return;
        }
        invulnerableCounter -= Time.deltaTime;
        if (invulnerableCounter <= 0)
        {
            isInvulnerable = false;
        }
    }

    private void NewGame()
    {
        currentHP = maxHP;
        // heart changed
        OnCharacterChange?.Invoke(this);
    }

    private void TriggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCounter = invulnerableDuration;
    }

    public void TakeDamage(Attack attacker)
    {
        if (isInvulnerable)
        {
            return;
        }
        if (attacker.damage < currentHP)
        {
            doHurt(attacker);
            return;
        }
        doDie();
    }

    private void doHurt(Attack attacker)
    {
        currentHP -= attacker.damage;
        TriggerInvulnerable();
        // invoke damage event
        OnTakeDamage?.Invoke(attacker.transform);
        OnCharacterChange?.Invoke(this);
    }

    private void doDie()
    {
        currentHP = 0;
        isDead = true;
        OnDead?.Invoke();
        OnCharacterChange?.Invoke(this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!dieInWater)
        {
            return;
        }

        if (other.CompareTag("Water"))
        {
            doDie();
        }
    }
}
