using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private Character character;
    private PlayerController playerController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        character = GetComponent<Character>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        SetAnimation();
    }

    // Set animation variables
    // Continuely detected state
    public void SetAnimation()
    {
        animator.SetFloat("velocityX", Math.Abs(rb.velocity.x));
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isGround", physicsCheck.isGround);
        animator.SetBool("isInvulnerable", character.isInvulnerable);
        animator.SetBool("isDead", character.isDead);
        animator.SetBool("isAttack", playerController.isAttack);
    }

    // Event trigger animations
    // Oncely
    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void PlayAttack()
    {
        animator.SetBool("isAttack", true);
        animator.SetTrigger("attack");
    }
}
