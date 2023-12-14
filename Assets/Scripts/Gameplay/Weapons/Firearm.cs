using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Firearm : Weapon
{
    [SerializeField] protected Transform shootPoint;

    public void LookAt(Transform Entity, Vector2 targetPosition)
    {
        float AngleRad = Mathf.Atan2(targetPosition.y - Entity.position.y, targetPosition.x - Entity.position.x);
        float AngleDeg = 180 / Mathf.PI * AngleRad;
        Entity.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    void LookAtMouse()
    {
        Vector2 mousePosition = MousePosition();
        LookAt(weaponVisual, mousePosition);
    }

    public override void Execute()
    {
        base.Execute();
        LookAtMouse();
    }
}
