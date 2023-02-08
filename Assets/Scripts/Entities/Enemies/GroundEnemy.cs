using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header(nameof(GroundEnemy))]
    [SerializeField] Transform walkPoint;
    [SerializeField] float walkRadius = 0.5f;

    void SnapToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 3f, groundMask);
        if (hit)
            transform.position = hit.point;
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
        SnapToGround();
        if (!DetectGround())
            transform.Rotate(Vector3.forward * -90);
    }
}
