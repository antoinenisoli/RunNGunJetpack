using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [SerializeField] Transform gunVisual;
    [SerializeField] protected CameraShake camShake = new CameraShake();
    protected AmmoSystem ammoSystem;

    private void Start()
    {
        ammoSystem = GetComponent<AmmoSystem>();
    }

    public override void Shoot()
    {
        if (!ammoSystem.CanShoot())
            return;

        camShake.Shake();
        ammoSystem.UseAmmo();
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet b = bulletObj.GetComponent<Bullet>();
        b.Shoot(gunVisual.right);
    }

    public override void ExecuteTimer()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LookAt(gunVisual, mousePosition);

        if (Input.GetMouseButton(0))
            shootTimer += Time.deltaTime;
        else
            shootTimer = fireRate;
    }
}
