using UnityEngine;

public class Snail : Enemy
{
    public override void Awake()
    {
        base.Awake();

        patrolState = new SnailWalkState();
        chaseState = new SnailHideSate();
    }

    public override void AfterWait()
    {
        // 不转身
        // 进入walk状态
        SwitchState(NPCState.Patrol);
    }
}
