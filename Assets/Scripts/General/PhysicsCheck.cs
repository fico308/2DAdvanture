using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{

    public bool isGround, isTouchLeft, isTouchRight;
    // overlaped layer
    public LayerMask layer;
    // detect radius
    public float groundRadius, leftRadius, rightRadius;
    public Vector2 groundOffset, leftOffset, rightOffset;

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
