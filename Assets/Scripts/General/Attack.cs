using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Base Variables")]
    public int damage;


    // FIXME: 当用户一直停留在怪物旁边, 需要持续性调用TakeDamage
    private void OnTriggerStay2D(Collider2D other)
    {
        // other touched this, other has been attacked
        // 这里参数必须使用Attacker, 受伤反弹需要用到attacker的位置
        other.GetComponent<Character>()?.TakeDamage(this); 
    }
}
