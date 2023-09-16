using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [Header(nameof(Gun))]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float fireRate;

    protected float shootTimer;
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public virtual void Shoot()
    {
        if (!enemy || !enemy.Target || !enemy.CanAttack())
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

    public override void Execute()
    {
        ExecuteTimer();
        if (shootTimer > fireRate)
        {
            shootTimer = 0;
            Shoot();
        }
    }
}
