using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string Name;
    public int Damages;
    public float BulletSpeed;

    public WeaponData(string name, int damages, float bulletSpeed)
    {
        Name = name;
        Damages = damages;
        BulletSpeed = bulletSpeed;
    }
}

public abstract class Weapon : MonoBehaviour
{
    [Header(nameof(Weapon))]
    [SerializeField] protected Transform weaponVisual;
    [SerializeField] bool lookAtMouse;
    private WeaponData weaponData;
    Camera cam;

    public virtual WeaponData WeaponData { get => weaponData; set => weaponData = value; }

    public virtual void Awake()
    {
        cam = Camera.main;
    }

    public void LookAt(Transform Entity, Vector2 targetPosition)
    {
        float AngleRad = Mathf.Atan2(targetPosition.y - Entity.position.y, targetPosition.x - Entity.position.x);
        float AngleDeg = 180 / Mathf.PI * AngleRad;
        Entity.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    public void FlipSprite()
    {
        bool flip = CameraManager.Instance.MousePosition().x < transform.position.x;
        if (flip)
            transform.localRotation = Quaternion.Euler(Vector3.right * 180);
        else
            transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public virtual void OnUpdate() { }

    public void Update()
    {
        if (lookAtMouse)
            FlipSprite();

        OnUpdate();
    }
}
