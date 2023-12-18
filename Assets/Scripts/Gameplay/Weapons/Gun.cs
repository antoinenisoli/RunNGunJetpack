using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum FireMode
{
    [Description("Semi-automatic")]
    SemiAutomatic = 0,
    [Description("Automatic")]
    Automatic = 1,
    [Description("Burst-Fire")]
    BurstFire = 2,
    [Description("Laser")]
    Laser = 3,
}

public class GunData : WeaponData
{
    public struct VisualData
    {
        public Sprite body, barrel, scope, stock;
    }

    public VisualData visualData = new VisualData();
    public int MagazineSize;
    public float FireRate;
    public FireMode FireMode;

    public GunData(string name, FireMode fireMode, int damages, float bulletSpeed, int magazineSize, float fireRate) : base(name, damages, bulletSpeed)
    {
        MagazineSize = magazineSize;
        FireRate = fireRate;
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
    [SerializeField] protected CameraShake camShake = new CameraShake();

    protected AmmoSystem ammoSystem;
    protected GunData gunData;
    protected float shootTimer;
    Enemy enemy;

    protected float FireRate => GunData.FireRate;
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

    public override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        ammoSystem = GetComponent<AmmoSystem>();
    }

    public virtual bool Shoot(bool useAmmo = true)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet b = bulletObj.GetComponent<Bullet>();
        Vector2 trajectory = new Vector2();

        if (enemy)
        {
            if (enemy.Target && enemy.CanAttack())
                trajectory = enemy.Target.transform.position - transform.position;
        }
        else
        {
            if (useAmmo)
                ammoSystem.UseAmmo();

            camShake.Shake();
            PlayerBullet bullet = b as PlayerBullet;
            bullet.Damage = GunData.Damages;
            bullet.Speed = GunData.BulletSpeed;
            trajectory = weaponVisual.right;
        }

        b.Shoot(trajectory.normalized);
        VFXManager.Instance.PlayVFX("PlayerGunShoot", shootPoint.position);
        return true;
    }

    public virtual void ExecuteTimer()
    {
        shootTimer += Time.deltaTime;
        if (GunData != null && ammoSystem.CanShoot() && !IsBlocked())
        {
            switch (GunData.FireMode)
            {
                case FireMode.SemiAutomatic: //semi auto
                    SemiAutomaticShoot();
                    break;
                case FireMode.Automatic: //auto
                    AutomaticShoot();
                    break;
                case FireMode.BurstFire: //burst-fire
                    BurstFire();
                    break;
            }
        }
    }

    protected void SemiAutomaticShoot()
    {
        if (Input.GetButtonDown("Fire1") && shootTimer > FireRate)
        {
            if (Shoot())
                shootTimer = 0;
        }
    }

    protected void AutomaticShoot()
    {
        if (Input.GetButton("Fire1"))
            shootTimer += Time.deltaTime;
        else
            shootTimer = FireRate;

        if (shootTimer > FireRate)
        {
            if (Shoot())
                shootTimer = 0;
        }
    }

    protected void BurstFire()
    {
        if (Input.GetButtonDown("Fire1") && shootTimer > FireRate)
        {
            shootTimer = 0;
            int randomBursts = Random.Range(3, 8);
            StartCoroutine(Burst(randomBursts, 0.06f));
        }
    }

    IEnumerator Burst(int amount, float fireDelay = 0.1f)
    {
        ammoSystem.UseAmmo();
        for (int i = 0; i < amount; i++)
            if (Shoot(false))
                yield return new WaitForSeconds(fireDelay);
    }

    public override void Execute()
    {
        ExecuteTimer();
    }
}
