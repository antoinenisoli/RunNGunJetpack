using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    [Header(nameof(Weapon))]
    [SerializeField] protected Transform weaponVisual;
    [SerializeField] protected Entity owner;
    [SerializeField] bool lookAtMouse;
    [SerializeField] protected ScriptableWeaponObject weaponDataObj;
    [SerializeField] protected UnityEvent OnAttack = new UnityEvent();
    WeaponData weaponData;
    Camera cam;

    public virtual WeaponData WeaponData { get => weaponData; set => weaponData = value; }

    public virtual void Awake()
    {
        cam = Camera.main;
        GetWeaponData();
    }

    public virtual void GetWeaponData()
    {
        WeaponData = new WeaponData("Weapon");
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
