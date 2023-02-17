using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public float force;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Push(Vector2 direction)
    {
        rb.AddForce(direction.normalized * force);
    }
}
