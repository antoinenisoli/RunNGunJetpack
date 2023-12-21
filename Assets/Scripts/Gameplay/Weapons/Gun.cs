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

public class Gun : Firearm
{
    [Header(nameof(Gun))]
    [SerializeField] protected GameObject bulletPrefab;

    protected AmmoSystem ammoSystem;
    protected GunData gunData;
    protected float shootTimer;

    protected float FireRate => GunData.AttackRate;
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

    public override void GetWeaponData()
    {
        if (weaponDataObj)
        {
            var obj = Instantiate(weaponDataObj);
            GunData = obj.GetGunData();
        }
    }

    public override bool Shoot(bool useAmmo = true)
    {
        base.Shoot(useAmmo);
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (ammoSystem && useAmmo)
            ammoSystem.UseAmmo();

        Vector2 trajectory = shootPoint.right;
        bullet.Shoot(trajectory.normalized, GunData.Damages, GunData.BulletSpeed);
        return true;
    }

    public void ExecuteTimer()
    {
        if (ammoSystem && !ammoSystem.CanShoot())
            return;

        if (GunData != null && !IsBlocked())
            ManageShooting();
    }

    protected void SemiAutomaticShoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > FireRate)
        {
            if (Shoot())
                shootTimer = 0;
        }
    }

    protected void AutomaticShoot()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > FireRate)
        {
            if (Shoot())
                shootTimer = 0;
        }
    }

    protected void BurstFire()
    {
        shootTimer += Time.deltaTime;
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

    public void ShootAt(Transform target)
    {
        LookAt(shootPoint, target.position);
        ExecuteTimer();
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
                break;

            case FireMode.BurstFire: //burst-fire
                BurstFire();
                break;
        }
    }
}
