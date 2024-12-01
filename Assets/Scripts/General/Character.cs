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
    [Header("Invulnerable")]
    public float invulnerableDuration;
    public float invulnerableCounter;
    public bool isInvulnerable;

    // Events
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDead;

    void Start()
    {
        currentHP = maxHP;
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
            currentHP -= attacker.damage;
            TriggerInvulnerable();
            // invoke damage event
            OnTakeDamage?.Invoke(attacker.transform);
            return;
        }
        currentHP = 0;
        isDead = true;
        OnDead?.Invoke();
    }
}
