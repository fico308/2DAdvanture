using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 faceDir;
    protected PhysicsCheck physicsCheck;


    [Header("Base Variables")]
    public float walkSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float backForce;

    [Header("Status")]
    public bool isHurt;
    public bool isDead;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();

        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // face和scale的方向是反的
        // FIXME: maybe localScale.x > 1
        faceDir = new Vector2(-transform.localScale.x, 0);
    }

    private void FixedUpdate()
    {
        if (isHurt || isDead)
        {
            return;
        }
        Move();
        TurnBack();
    }

    protected virtual void TurnBack()
    {
        if ((physicsCheck.isTouchLeft && faceDir.x < 0) || (physicsCheck.isTouchRight && faceDir.x > 0))
        {
            transform.localScale = new Vector3(faceDir.x, transform.localScale.y, transform.localScale.z);
        }
    }

    protected virtual void Move()
    {
        rb.velocity = new Vector2(faceDir.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

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

}
