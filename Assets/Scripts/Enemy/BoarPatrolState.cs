
public class BoarPatrolState : BaseState
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
            enemy.isWait = true;
            enemy.animator.SetBool("isWalk", false);
        } else {
            enemy.isWait = false;
            enemy.animator.SetBool("isWalk", true);
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
