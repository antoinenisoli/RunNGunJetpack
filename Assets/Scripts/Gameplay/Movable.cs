using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public float Weight = 10f;
    public float force;
    [SerializeField] Vector2 forceLimits = new Vector2(100, 5000);
    [SerializeField] float collisionRadius = 1.5f;
    [SerializeField] LayerMask destroyables;
    [SerializeField] [Curve(1, 250)] AnimationCurve impactCurve;
    [SerializeField] List<Collider2D> neighbours = new List<Collider2D>();

    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    private void OnDrawGizmos()
    {
        if (rb)
            Handles.Label(transform.position, rb.velocity.sqrMagnitude.ToString());

        Gizmos.DrawWireSphere(transform.position, collisionRadius);
    }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void GetTouchingColliders()
    {
        var childOverlap = OverlapBox(1 << gameObject.layer);
        neighbours = childOverlap.ToList();
        if (neighbours.Contains(boxCollider))
            neighbours.Remove(boxCollider);
    }

    public List<Collider2D> GetNeighbours() => neighbours;

    public void Push(Vector2 direction)
    {
        rb.AddForce(direction.normalized * force);
    }

    int ComputeCollisionDamages(float force)
    {
        var coeff = Mathf.InverseLerp(forceLimits.x, forceLimits.y, force);
        return Mathf.RoundToInt(impactCurve.Evaluate(coeff));
    }

    Collider2D[] OverlapBox(LayerMask mask)
    {
        return Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.size, 0, mask);
    }

    private void ManageCollisions()
    {
        var overlap = OverlapBox(destroyables);
        foreach (var item in overlap)
        {
            Entity entity = item.GetComponentInChildren<Entity>();
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

    private void FixedUpdate()
    {
        GetTouchingColliders();
        ManageCollisions();
    }
}
