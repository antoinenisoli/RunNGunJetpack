using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : WeaponData
{
    public struct VisualData
    {
        public Sprite body, barrel, scope, stock;
    }

    public VisualData visualData = new VisualData();
    public int MagazineSize;
    public int FireMode;

    public GunData(string name, int damages, float bulletSpeed, int magazineSize, int fireMode) : base(name, damages, bulletSpeed)
    {
        MagazineSize = magazineSize;
        FireMode = fireMode;
        visualData = new VisualData();
    }

    public void SetSprites(Sprite body, Sprite barrel, Sprite scope, Sprite stock)
    {
        visualData.body = body;
        visualData.barrel = barrel;
        visualData.scope = scope;
        visualData.stock = stock;
    }
}


public class Gun : Firearm
{
    [Header(nameof(Gun))]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float fireRate;

    protected AmmoSystem ammoSystem;
    protected GunData gunData;
    protected float shootTimer;
    Enemy enemy;

    public AmmoSystem AmmoSystem { get => ammoSystem; set => ammoSystem = value; }
    public GunData GunData { get => gunData; 
        set
        {
            gunData = value;
            WeaponData = value;
        }
    }

    public override WeaponData WeaponData { get => GunData; 
        set
        {
            base.WeaponData = value;
            gunData = value as GunData;
        }
    }

    public virtual void Awake()
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
