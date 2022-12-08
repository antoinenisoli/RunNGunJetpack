using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }
}
