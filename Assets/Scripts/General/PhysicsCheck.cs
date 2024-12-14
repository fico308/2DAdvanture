using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    [Header("Display State")]
    public bool isGround;
    public bool isTouchLeft;
    public bool isTouchRight;
    // overlaped layer
    [Header("Base variables")]
    public LayerMask layer;
    // detect radius
    public float groundRadius = 0.1f;
    public float leftRadius = 0.1f;
    public float rightRadius = 0.1f;
    public Vector2 groundOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    private void Start()
    {
        // TODO: automated offset
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + groundOffset * transform.localScale.x, groundRadius, layer);
        isTouchLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, leftRadius, layer);
        isTouchRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, rightRadius, layer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + groundOffset * transform.localScale.x, groundRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, leftRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, rightRadius);
    }
}
