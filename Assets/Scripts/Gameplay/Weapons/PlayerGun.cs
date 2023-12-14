using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [Header(nameof(PlayerGun))]
    [SerializeField] RandomGunGenerator randomGunGenerator;
    [SerializeField] protected CameraShake camShake = new CameraShake();

    public override void Awake()
    {
        base.Awake();
        ammoSystem = GetComponent<AmmoSystem>();
        SetGunData(randomGunGenerator.Generate());
    }

    public override void Shoot()
    {
        if (!ammoSystem.CanShoot())
            return;

        camShake.Shake();
        ammoSystem.UseAmmo();
        GameObject newB = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        PlayerBullet bullet = newB.GetComponent<PlayerBullet>();
        bullet.Shoot(weaponVisual.right);
        bullet.Damage = GunData.Damages;
        bullet.Speed = GunData.BulletSpeed;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void ExecuteTimer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LookAt(weaponVisual, mousePosition);

        if (Input.GetButtonDown("Fire1"))
            shootTimer += Time.deltaTime;
        else
            shootTimer = fireRate;
    }

    public void SetGunData(GunData newData)
    {
        GunData = newData;
        randomGunGenerator.gunSprite.SetSprites(GunData.visualData);
        AmmoSystem.MaxAmmo = newData.MagazineSize;
        AmmoSystem.AmmoAmount = AmmoSystem.MaxAmmo;
    }
}
