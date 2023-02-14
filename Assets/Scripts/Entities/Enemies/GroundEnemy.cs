using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header(nameof(GroundEnemy))]
    [SerializeField] float walkRadius = 0.5f;
    [SerializeField] Transform groundDetector, wallDetector;

    public override void Awake()
    {
        base.Awake();
        SnapToGround();
    }

    void SnapToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 3f, groundMask);
        if (hit)
            transform.position = hit.point;
    }

    bool DetectGround()
    {
        bool hit = Physics2D.OverlapCircle(groundDetector.position, walkRadius, groundMask);
        return hit;
    }

    bool DetectWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallDetector.position, wallDetector.right, 1f, groundMask);
        return hit;
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
        if (DetectWall())
            transform.Rotate(Vector3.forward * 90);
    }
}
