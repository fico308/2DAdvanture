using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    public Transform playerTransform;
    public GameObject spirteRender;

    public bool canPress;

    private PlayerInputControl inputControl;
    private Animator animator;
    private IInterctable interctable;

    private void Awake()
    {
        animator = spirteRender.GetComponent<Animator>();
        // 重新实例化一个
        inputControl = new PlayerInputControl();
        inputControl.Enable();
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += ActionChange;
        inputControl.GamePlay.Confirm.started += Confirm;
    }



    private void Update()
    {
        spirteRender.transform.localScale = playerTransform.localScale;
    }

    private void Active()
    {
        spirteRender.SetActive(true);
        animator.Play("E");
        canPress = true;
    }

    private void Inactive()
    {
        spirteRender.SetActive(false);
        canPress = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interctable"))
        {
            Active();
            interctable = other.GetComponent<IInterctable>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interctable"))
        {
            Inactive();
        }
    }

    private void ActionChange(object obj, InputActionChange actionChange)
    {
        if (actionChange == InputActionChange.ActionStarted)
        {
            var d = ((InputAction)obj).activeControl.device;
            switch (d)
            {
                case Keyboard:
                    // play keyboard animation
                    break;
                case XInputController:
                    Debug.Log("xbox");
                    break;
                case DualShockGamepad:
                    Debug.Log("ps");
                    break;
            }
        }
    }

    private void Confirm(InputAction.CallbackContext context)
    {
        if (canPress)
        {
            interctable?.TakeAction();
            GetComponent<AudioSender>()?.PlayAudioClip();
            canPress = false;
        }

    }
}
