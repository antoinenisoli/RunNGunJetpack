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

[System.Serializable]
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

    protected AmmoSystem ammoSystem;
    protected GunData gunData;
    protected float shootTimer;

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
        ammoSystem = GetComponent<AmmoSystem>();
    }

    public override bool Shoot(bool useAmmo = true)
    {
        base.Shoot(useAmmo);
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (ammoSystem && useAmmo)
            ammoSystem.UseAmmo();

        bullet.Damage = GunData.Damages;
        bullet.Speed = GunData.BulletSpeed;
        Vector2 trajectory = weaponVisual.right;

        bullet.Shoot(trajectory.normalized);
        return true;
    }

    public void ExecuteTimer()
    {
        if (ammoSystem && !ammoSystem.CanShoot())
            return;

        if (GunData != null && !IsBlocked())
        {
            shootTimer += Time.deltaTime;
            ManageShooting();
        }
    }

    protected void SemiAutomaticShoot()
    {
        if (shootTimer > FireRate)
        {
            if (Shoot())
                shootTimer = 0;
        }
    }

    protected void AutomaticShoot()
    {
        shootTimer += Time.deltaTime;
    }

    protected void BurstFire()
    {
        if (shootTimer > FireRate)
        {
            shootTimer = 0;
            int randomBursts = Random.Range(3, 8);
            StartCoroutine(Burst(randomBursts, 0.06f));
        }
    }

    protected IEnumerator Burst(int amount, float fireDelay = 0.1f)
    {
        if (ammoSystem)
            ammoSystem.UseAmmo();

        for (int i = 0; i < amount; i++)
            if (Shoot(false))
                yield return new WaitForSeconds(fireDelay);
    }

    public virtual void ManageShooting()
    {
        switch (GunData.FireMode)
        {
            case FireMode.SemiAutomatic: //semi auto
                SemiAutomaticShoot();
                break;

            case FireMode.Automatic: //auto
                AutomaticShoot();
                if (shootTimer > FireRate)
                {
                    if (Shoot())
                        shootTimer = 0;
                }

                break;

            case FireMode.BurstFire: //burst-fire
                BurstFire();
                break;
        }
    }
}
