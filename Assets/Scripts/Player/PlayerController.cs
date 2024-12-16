using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Read input
    private PlayerInputControl playerInputActions;
    private Vector2 inputDirection;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimationController animationController;

    // Internal states
    private bool isHurt;
    private SceneType currenSceneType;
    [Header("Event Listener")]
    public SceneLoadEventSO newSceneLoadEvent;
    public VoidEventSO sceneLoadedEvent;

    [Header("State")]
    public bool isAttack;

    [Header("Base Variables")]
    public float moveSpeed;
    public float jumpForce;
    public float backForce;

    public PhysicsMaterial2D normal, solid;

    private void Awake()
    {
        playerInputActions = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        animationController = GetComponent<PlayerAnimationController>();

        // register jump event
        playerInputActions.GamePlay.Jump.started += Jump;
        // attack event
        playerInputActions.GamePlay.Attack.started += Attack;
    }


    private void OnEnable()
    {
        playerInputActions.Enable();
        newSceneLoadEvent.OnEventRaised += OnNewSceneLoad;
        sceneLoadedEvent.OnEventRaised += OnSceneLoaded;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        newSceneLoadEvent.OnEventRaised -= OnNewSceneLoad;
        sceneLoadedEvent.OnEventRaised -= OnSceneLoaded;
    }


    private void Update()
    {
        inputDirection = playerInputActions.GamePlay.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (isHurt || isAttack)
        {
            return;
        }
        Move();
        updateMeterial();
    }

    private void updateMeterial()
    {
        rb.sharedMaterial = physicsCheck.isGround ? normal : solid;
    }

    public void Move()
    {
        // move
        rb.velocity = new Vector2(inputDirection.x * moveSpeed * Time.deltaTime, rb.velocity.y);

        // flip player face
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            // front
            faceDir = Math.Abs(faceDir);
        }
        else if (inputDirection.x < 0)
        {
            // back
            faceDir = -Math.Abs(faceDir);
        }
        transform.localScale = new Vector3(faceDir, transform.localScale.y, transform.localScale.z);
    }

    public void HurtStart(Transform attacker)
    {
        isHurt = true;
        // stop move
        rb.velocity = Vector2.zero;
        // move back
        Vector2 backDir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(backDir * backForce, ForceMode2D.Impulse);
    }

    public void HurtEnd()
    {
        isHurt = false;
    }

    public void Dead()
    {
        playerInputActions.GamePlay.Disable();
    }

    public void AttackStart()
    {
        isAttack = true;
    }

    public void AttackEnd()
    {
        isAttack = false;
    }

    #region Key Events
    private void Jump(InputAction.CallbackContext context)
    {
        // is allow jump if player has been hurting
        if (isHurt)
        {
            return;
        }
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        // stop move
        // rb.velocity = Vector2.zero;

        // attack
        AttackStart();
        animationController.PlayAttack();
    }
    #endregion

    #region Event Listener
    private void OnNewSceneLoad(GameSceneSO newScene, Vector3 arg1, bool arg2)
    {
        // 新场景开始加载
        // 屏蔽游戏输入
        Debug.Log($"Player new scene load {newScene}");
        rb.constraints = RigidbodyConstraints2D.FreezeAll; // 禁止移动
        playerInputActions.GamePlay.Disable();
        currenSceneType = newScene.type;

    }
    private void OnSceneLoaded()
    {
        Debug.Log("Player new scene loaded");
        if (currenSceneType == SceneType.Game)
        {
            playerInputActions.GamePlay.Enable();
        }
        // gameObject.SetActive(true);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    #endregion
}
