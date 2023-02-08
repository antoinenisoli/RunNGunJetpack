using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float fireRate;

    protected float shootTimer;
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void LookAt(Transform Entity, Vector2 targetPosition)
    {
        float AngleRad = Mathf.Atan2(targetPosition.y - Entity.position.y, targetPosition.x - Entity.position.x);
        float AngleDeg = 180 / Mathf.PI * AngleRad;
        Entity.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    public virtual void Shoot()
    {
        if (!enemy || !enemy.Target)
            return;

        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet b = bulletObj.GetComponent<Bullet>();
        Vector2 trajectory = enemy.Target.transform.position - transform.position;
        b.Shoot(trajectory.normalized);
    }

    public virtual void ExecuteTimer()
    {
        shootTimer += Time.deltaTime;
    }

    private void Update()
    {
        if (!enemy || !enemy.Target)
            return;

        ExecuteTimer();
        if (shootTimer > fireRate)
        {
            shootTimer = 0;
            Shoot();
        }
    }
}
