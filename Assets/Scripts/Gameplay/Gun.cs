using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform gunVisual;
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;
    float shootTimer;

    void LookAt(Transform Entity, Vector2 targetPosition)
    {
        float AngleRad = Mathf.Atan2(targetPosition.y - Entity.position.y, targetPosition.x - Entity.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        // Rotate Object
        Entity.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LookAt(gunVisual, mousePosition);

        if (Input.GetMouseButton(0))
        {
            shootTimer += Time.deltaTime;
            if (shootTimer > fireRate)
            {
                shootTimer = 0;
                GameObject bulletObj = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                Bullet b = bulletObj.GetComponent<Bullet>();
                b.Shoot(gunVisual.right);
            }
        }
        else
            shootTimer = fireRate;
    }
}
