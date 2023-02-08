using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Team
{
    Player,
    Enemy,
    Neutral,
}

public class Bullet : MonoBehaviour
{
    [SerializeField] Team myTeam;
    [SerializeField] GameObject HitFX;
    [SerializeField] protected int damageAmount = 1;
    [SerializeField] protected float speed = 5f, lifeTime = 3f;
    [SerializeField] protected bool lookAtTarget;
    [SerializeField] LayerMask obstacleMask;
    protected Vector2 trajectory;

    private void Awake()
    {
        //SoundManager.Instance.PlayAudio(soundName);
    }

    public void Shoot(Vector2 trajectory)
    {
        this.trajectory = trajectory.normalized;
        Destroy(gameObject, lifeTime);
    }

    void SelfDestroy(float delay = 0)
    {
        if (HitFX)
            Instantiate(HitFX, transform.position, Quaternion.identity);

        Destroy(gameObject, delay);
    }

    public virtual void Collision(Collider2D collision)
    {
        if (GameDevHelper.Contains(obstacleMask, collision.gameObject.layer))
        {
            SelfDestroy();
            return;
        }

        Entity entity = collision.GetComponentInParent<Entity>();
        if (entity)
        {
            switch (myTeam)
            {
                case Team.Player:
                    if (entity is PlayerController)
                        return;

                    break;

                case Team.Enemy:
                    if (entity is Enemy)
                        return;

                    break;

                case Team.Neutral:
                    break;
            }

            entity.TakeDamage(damageAmount);
            SelfDestroy();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Collision(collision);
    }

    void LookAtTrajectory()
    {
        float angle = Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Update()
    {
        if (lookAtTarget)
            LookAtTrajectory();

        transform.position += (Vector3)trajectory * Time.deltaTime * speed;
    }
}
