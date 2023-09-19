using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Block : Entity
{
    public float force;
    [SerializeField] Vector2 forceLimits = new Vector2(100, 5000);
    [SerializeField] float collisionRadius = 1.5f;
    [SerializeField] [Curve(1, 250)] AnimationCurve impactCurve;
    [SerializeField] LayerMask destroyables;

    Rigidbody2D rb;

    private void OnDrawGizmos()
    {
        if (rb)
            Handles.Label(transform.position, rb.velocity.sqrMagnitude.ToString());

        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Push(Vector2 direction)
    {
        rb.AddForce(direction.normalized * force);
    }

    int ComputeCollisionDamages(float force)
    {
        var coeff = Mathf.InverseLerp(forceLimits.x, forceLimits.y, force);
        return Mathf.RoundToInt(impactCurve.Evaluate(coeff));
    }

    private void FixedUpdate()
    {
        int maxColliders = 10;
        Collider2D[] hitColliders = new Collider2D[maxColliders];
        int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, collisionRadius, hitColliders, destroyables);
        for (int i = 0; i < numColliders; i++)
        {
            Entity entity = hitColliders[i].GetComponentInChildren<Entity>();
            if (entity && entity != this)
            {
                int force = Mathf.RoundToInt(rb.velocity.sqrMagnitude);            
                if (rb.gravityScale > 0 && rb.velocity.sqrMagnitude > forceLimits.x)
                {
                    int damages = ComputeCollisionDamages(force);
                    print("collision force = " + force + " computed damages = " + damages);
                    entity.TakeDamage(damages);
                    rb.velocity = new Vector2();
                }
            }
        }
    }
}
