using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header(nameof(Enemy))]
    [SerializeField] protected float speed;
    [SerializeField] protected int contactDamage = 5;
    [SerializeField] protected float contactPush = 1000f;
    protected Rigidbody2D rb;
    [SerializeField] protected Entity target;
    [SerializeField] protected LayerMask groundMask;

    public Entity Target { get => target; }

    public void SetTarget(Entity target)
    {
        this.target = target;
    }

    public bool CanAttack()
    {
        if (!target)
            return false;

        bool blocked = Physics2D.Linecast(transform.position, target.transform.position, groundMask);
        return !blocked;
    }

    private void OnDestroy()
    {
        VFXManager.Instance.PlayVFX("ExplodeFX", transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInChildren<PlayerController>();
        
        if (player)
        {
            Vector2 dir = player.transform.position - transform.position;
            player.TakeDamage(contactDamage);
            player.Push(contactPush, dir.normalized);
        }
    }

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }
}
