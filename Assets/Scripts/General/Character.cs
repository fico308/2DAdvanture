using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
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
    public VoidEventSO newGameEvent; // 不能用newGame来初始化ememy血量, 因为这个敌人可能不在第一个场景

    private void OnEnable()
    {
        Debug.Log($"Enable {GetID().ID}");
        newGameEvent.OnEventRaised += NewGame;
        if (!gameObject.CompareTag("Player"))
        {
            // 非player直接满血
            currentHP = maxHP;
        }
        // 放在这里时因为只有当前场景加载出来之后其中的对象才需要注册 
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnregisterSaveData();
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
        Debug.Log($"New Game {GetID().ID}");
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

    public DataDefination GetID()
    {
        return GetComponent<DataDefination>();
    }

    public void Save(SaveData storage)
    {
        var id = GetID().ID;
        // if (id == string.Empty)
        // {
        //     // 不需要保存?
        //     return;
        // }
        var cd = new SaveData.CharacterData();
        cd.ID = id;
        cd.hp = currentHP;
        cd.positionX = transform.position.x;
        cd.positionY = transform.position.y;
        cd.positionZ = transform.position.z;

        storage.characters[id] = cd;

        // if (storage.characters.ContainsKey(id))
        // {
        //     storage.characters[id] = ;
        // }
        // else
        // {
        //     storage.characters.Add(id, transform.position);
        // }

    }

    public void Load(SaveData storage)
    {
        var id = GetID().ID;
        if (storage.characters.TryGetValue(id, out var data))
        {
            Debug.Log($"load {id} data: {data}");
            currentHP = data.hp;
            transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
            OnCharacterChange?.Invoke(this);
        }
    }
}
