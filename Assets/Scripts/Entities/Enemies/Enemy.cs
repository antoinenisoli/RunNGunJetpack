using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] float speed;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 newVelocity = transform.right * speed;
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }
}
