using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [SerializeField] Transform walkPoint;
    [SerializeField] float walkRadius = 0.5f;
    [SerializeField] LayerMask groundMask;

    void SnapToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000f, groundMask);
        if (hit)
            transform.position = hit.point;
    }

    public override void Awake()
    {
        base.Awake();
        SnapToGround();
    }

    bool DetectGround()
    {
        return Physics2D.OverlapCircle(walkPoint.position, walkRadius, groundMask);
    }

    private void Update()
    {
        Vector2 newVelocity = transform.right * speed;
        rb.velocity = newVelocity;
    }

    private void FixedUpdate()
    {
        if (!DetectGround())
            transform.Rotate(Vector3.forward * -90);
    }
}
