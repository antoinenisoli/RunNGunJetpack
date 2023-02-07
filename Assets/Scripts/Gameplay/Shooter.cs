using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float fireRate;
    [SerializeField] protected CameraShake camShake = new CameraShake();

    protected float shootTimer;

    public void LookAt(Transform Entity, Vector2 targetPosition)
    {
        float AngleRad = Mathf.Atan2(targetPosition.y - Entity.position.y, targetPosition.x - Entity.position.x);
        float AngleDeg = 180 / Mathf.PI * AngleRad;
        Entity.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    public virtual void Shoot()
    {
        camShake.Shake();
        GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
    }

    public virtual void ExecuteTimer()
    {
        shootTimer += Time.deltaTime;
    }

    private void Update()
    {
        ExecuteTimer();

        if (shootTimer > fireRate)
        {
            shootTimer = 0;
            Shoot();
        }
    }
}
