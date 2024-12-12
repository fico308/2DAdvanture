using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailHideSate : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy;

        enemy.currentSpeed = enemy.chaseSpeed;
        enemy.animator.SetBool("needHide", true);
        enemy.isWait = true;
    }

    public override void LoginUpdate()
    {
        
    }


    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        enemy.isWait = false;
        enemy.animator.SetBool("needHide", false);
    }

}
