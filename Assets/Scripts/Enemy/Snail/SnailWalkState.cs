using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailWalkState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        this.enemy = enemy;

        enemy.currentSpeed = enemy.walkSpeed;
        enemy.animator.SetBool("isWalk", true);
    }
    public override void LoginUpdate()
    {
        if (!enemy.physicsCheck.isGround || (enemy.physicsCheck.isTouchLeft && enemy.faceDir.x < 0) || (enemy.physicsCheck.isTouchRight && enemy.faceDir.x > 0))
        {
            // 直接转身
            enemy.TurnAround();
        }
    }

    public override void PhysicsUpdate()
    {
        if (enemy.FoundPlayer())
        {
            // switch to chase state
            enemy.SwitchState(NPCState.Chase);
        }
    }


    public override void OnExit()
    {
        enemy.animator.SetBool("isWalk", false);
    }
}
