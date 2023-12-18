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
    [SerializeField] GameObject HitFX;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] protected float lifeTime = 3f;

    protected Vector2 trajectory;
    float speed = 50f;
    int damage = 1;

    public virtual int Damage { get => damage; set => damage = value; }
    public virtual float Speed { get => speed; set => speed = value; }

    public void Shoot(Vector2 trajectory)
    {
        this.trajectory = trajectory.normalized;
        Destroy(gameObject, lifeTime);
    }

    public void SelfDestroy(float delay = 0)
    {
        Destroy(gameObject, delay);
    }

    private void OnDestroy()
    {
        if (HitFX)
            Instantiate(HitFX, transform.position, Quaternion.identity);
    }

    public abstract bool CantTakeDamage(Entity entity);

    public virtual void Collision(Collider2D collision)
    {
        Entity entity = collision.GetComponentInParent<Entity>();
        if (entity)
        {
            if (CantTakeDamage(entity))
                return;

            entity.TakeDamage(Damage);
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
        transform.position += (Vector3)trajectory * Time.deltaTime * Speed;
    }
}
