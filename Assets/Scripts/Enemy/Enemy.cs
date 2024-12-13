using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PhysicsCheck physicsCheck;
    public Vector2 faceDir;


    [Header("Base Variables")]
    public float walkSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float backForce;
    public float waitDuration;

    [Header("Detect Player")]
    public Vector2 centerOffset;
    public Vector2 boxSize;
    public float detectDistance;
    public LayerMask detectLayer;
    public float chaseDuration;
    public float chaseCounter;

    [Header("Status")]
    public bool isHurt;
    public bool isDead;
    public bool isWait;

    private float waitCounter;

    private BaseState currentState;
    // 各种敌人都有patrol和chase
    protected BaseState patrolState;
    protected BaseState chaseState;

    // 子类复写并初始化状态
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = walkSpeed;
        waitCounter = waitDuration;
    }

    private void OnEnable()
    {
        // 开始时默认为patrol
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    // Update is called once per frame
    private void Update()
    {
        // face和scale的方向是反的
        // FIXME: maybe localScale.x > 1
        faceDir = new Vector2(-transform.localScale.x, 0);

        currentState.LoginUpdate();
        WaitCounter(); // 这里会改变faceDir, 导致LogicUpdate出问题, 所以LoginUpdate必须放在WaitCounter上面
    }

    private void FixedUpdate()
    {
        if (!isWait && !isHurt && !isDead && physicsCheck.isGround)
        {
            Move();
        }
        // else
        // {
        //     // IDEA: Not sure is it a good idea
        //     rb.velocity = new Vector2(0, rb.velocity.y);
        // }
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    private void WaitCounter()
    {
        if (isWait)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                isWait = false;
                waitCounter = waitDuration;
                AfterWait();
            }
        }
    }


    protected void Move()
    {
        rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    public virtual void AfterWait()
    {
        // 转身
        TurnAround();
    }

    public void TurnAround()
    {
        transform.localScale = new Vector3(faceDir.x, transform.localScale.y, transform.localScale.z);
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, boxSize, 0, faceDir, detectDistance, detectLayer);
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase => chaseState,
            _ => patrolState
        };
        // end old
        currentState.OnExit();

        // init new
        currentState = newState;
        currentState.OnEnter(this);
    }

    // private void OnDrawGizmosSelected() {
    //      Gizmos.DrawWireCube(transform.position + (Vector3)centerOffset + transform.forward * detectDistance, boxSize);
    // }

    #region Events

    public void TakeDamage(Transform attacker)
    {
        // 播放攻击动画, 阻止移动
        isHurt = true;
        animator.SetBool("isHurt", true);
        animator.SetTrigger("hurt");
        // 往attacker的反方向后退一段距离
        Vector2 faceDir = new Vector2(
            // if attacker on the left, v = (1, 0)
            // else v = (-1, 0)
            attacker.position.x < transform.position.x ? Math.Abs(transform.localScale.x) : -Math.Abs(transform.localScale.x), 0);

        // p:e   faceDir = (1, 0) ->
        // e:p   faceDir = (-1, 0) <-
        rb.AddForce(faceDir * backForce, ForceMode2D.Impulse);
        Debug.Log(backForce);

        // 立即进入追击状态
        SwitchState(NPCState.Chase);
        StartCoroutine(AfterHurt(faceDir));
    }

    IEnumerator AfterHurt(Vector2 dir)
    {
        yield return new WaitForSeconds(0.4f);
        // 如果从背后攻击, 翻转方向
        transform.localScale = new Vector3(dir.x, transform.localScale.y, transform.localScale.z);
        isHurt = false;
        animator.SetBool("isHurt", false);
    }


    public void OnDead()
    {
        // 设置为Ignore Raycast layer
        gameObject.layer = 2;
        isDead = true;
        animator.SetBool("isDead", true);
    }


    public void DestroyAfterAnimation()
    {
        Destroy(this.gameObject);
    }
    # endregion
}
