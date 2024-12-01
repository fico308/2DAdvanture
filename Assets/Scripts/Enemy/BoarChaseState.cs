using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{


    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("chase");
        this.enemy = enemy;

        enemy.currentSpeed = enemy.chaseSpeed;
        enemy.animator.SetBool("isRun", true);
    }

    public override void LoginUpdate()
    {
        if (!enemy.physicsCheck.isGround || (enemy.physicsCheck.isTouchLeft && enemy.faceDir.x < 0) || (enemy.physicsCheck.isTouchRight && enemy.faceDir.x > 0))
        {
            // 碰到墙立即返回
            enemy.transform.localScale = new Vector3(enemy.faceDir.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
        }
        ChaseCounter();
    }

    public override void PhysicsUpdate()
    {
    }

    public override void OnExit()
    {
        enemy.animator.SetBool("isRun", false);
    }

    private void ChaseCounter()
    {
        if (!enemy.FoundPlayer())
        {
            enemy.chaseCounter -= Time.deltaTime;
            if (enemy.chaseCounter <= 0)
            {
                enemy.SwitchState(NPCState.Patrol);
                enemy.chaseCounter = enemy.chaseDuration;
            }
        }
    }
}
