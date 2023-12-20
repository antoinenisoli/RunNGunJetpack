using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Team
{
    Player,
    Enemy,
    Neutral,
}

public abstract class Bullet : MonoBehaviour
{
    [Header(nameof(Bullet))]
    [SerializeField] string HitFX;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] protected float lifeTime = 3f;
    [SerializeField] bool affectedByGravity;

    protected Vector2 trajectory;
    float speed = 50f;
    int damage = 1;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (affectedByGravity)
            rb.velocity = trajectory * speed;
    }

    public void Shoot(Vector2 trajectory, int damage, float speed)
    {
        this.trajectory = trajectory.normalized;
        this.speed = speed;
        this.damage = damage;
        Destroy(gameObject, lifeTime);
    }

    public void SelfDestroy(float delay = 0)
    {
        Destroy(gameObject, delay);
    }

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(HitFX))
            VFXManager.Instance.PlayVFX(HitFX, transform.position);
    }

    public abstract bool CantTakeDamage(Entity entity);

    public virtual void Collision(Collider2D collision)
    {
        Entity entity = collision.GetComponentInParent<Entity>();
        if (entity)
        {
            if (CantTakeDamage(entity))
                return;

            entity.TakeDamage(damage);
            SelfDestroy();
        }

        if (GameDevHelper.Contains(obstacleMask, collision.gameObject.layer))
            SelfDestroy();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collision(collision);
    }

    public virtual void Update()
    {
        if (!affectedByGravity)
            rb.velocity = trajectory * speed;

        transform.right = rb.velocity.normalized;
    }
}
