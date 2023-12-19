using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    [Header(nameof(Enemy))]
    [SerializeField] protected float speed;
    [SerializeField] protected int contactDamage = 5;
    [SerializeField] protected float contactPush = 1000f;
    [SerializeField] protected Entity target;
    [SerializeField] protected LayerMask groundMask;
    protected Rigidbody2D rb;

    public Entity Target { get => target; }

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

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

    public override void Death()
    {
        base.Death();
        VFXManager.Instance.PlayVFX("BloodFX", transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInChildren<PlayerController>();   
        if (player)
            PlayerTouched(player);
    }

    void PlayerTouched(PlayerController player)
    {
        print(name + " collided with player!");
        player.TakeDamage(contactDamage);

        /*Vector2 dir = player.transform.position - transform.position;      
        player.Push(contactPush, dir.normalized);*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponentInChildren<PlayerController>();
        if (player)
            PlayerTouched(player);
    }
}
