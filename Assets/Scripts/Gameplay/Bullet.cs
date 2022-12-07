using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject HitFX;
    [SerializeField] protected float damageAmount = 1f;
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
            SelfDestroy();

        /*Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy)
        {
            enemy.TakeDmg(damageAmount);
            SelfDestroy();
        }*/
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
