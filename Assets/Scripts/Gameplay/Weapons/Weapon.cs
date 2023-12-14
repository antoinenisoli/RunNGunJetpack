using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public string Name;
    public int Damages, Range;

    public WeaponData(string name, int damages, int range)
    {
        Name = name;
        Damages = damages;
        Range = range;
    }
}

public abstract class Weapon : MonoBehaviour
{
    [Header(nameof(Weapon))]
    [SerializeField] protected Transform weaponVisual;
    private WeaponData weaponData;

    public virtual WeaponData WeaponData { get => weaponData; set => weaponData = value; }

    public Vector2 MousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void LookAt(Transform Entity, Vector2 targetPosition)
    {
        float AngleRad = Mathf.Atan2(targetPosition.y - Entity.position.y, targetPosition.x - Entity.position.x);
        float AngleDeg = 180 / Mathf.PI * AngleRad;
        Entity.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    public virtual void Execute()
    {
        Vector2 mousePosition = MousePosition();
        LookAt(weaponVisual, mousePosition);
    }

    private void Update()
    {
        Execute();
    }
}
