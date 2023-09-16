using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header(nameof(GroundEnemy))]
    [SerializeField] [Range(-1,1)] int moveDirection = 1;

    private void Update()
    {
        Vector2 newVelocity = rb.velocity;
        newVelocity.x = moveDirection * speed;
        rb.velocity = newVelocity;
    }
}
