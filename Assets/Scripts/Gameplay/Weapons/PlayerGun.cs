using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [Header(nameof(PlayerGun))]
    [SerializeField] RandomGunGenerator randomGunGenerator;
    [SerializeField] CameraShake camShake;

    public override WeaponData WeaponData 
    { 
        get => base.WeaponData; 
        set => base.WeaponData = value; 
    }

    public override void Awake()
    {
        base.Awake();
        SetGunData(randomGunGenerator.Generate());
    }

    public override bool Shoot(bool useAmmo = true)
    {
        camShake.Shake();
        return base.Shoot(useAmmo);
    }

    public void SetGunData(GunData newData)
    {
        GunData = newData;
        randomGunGenerator.gunSprite.SetSprites(GunData.visualData);
        AmmoSystem.MaxAmmo = newData.MagazineSize;
        AmmoSystem.AmmoAmount = AmmoSystem.MaxAmmo;
    }

    protected void AutomaticShoot()
    {
        if (Input.GetButton("Fire1"))
            shootTimer += Time.deltaTime;
        else
            shootTimer = 0;

        if (shootTimer > FireRate)
        {
            if (Shoot())
                shootTimer = 0;
        }
    }

    public override void ManageShooting()
    {
        switch (GunData.FireMode)
        {
            case FireMode.SemiAutomatic: //semi auto
                if (Input.GetButtonDown("Fire1"))
                    SemiAutomaticShoot();

                break;

            case FireMode.Automatic: //auto
                AutomaticShoot();
                break;

            case FireMode.BurstFire: //burst-fire
                if (Input.GetButtonDown("Fire1"))
                    BurstFire();

                break;
        }
    }

    public override void OnUpdate()
    {
        ExecuteTimer();
    }
}
