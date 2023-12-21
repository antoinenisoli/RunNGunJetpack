using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propulsion : PlayerCapacity
{
    public override ModuleType myType => ModuleType.Propulsion;

    [Header(nameof(Propulsion))]
    [SerializeField] float propulsionForce = 15f;

    public override void Effect()
    {
        Player.ResetYVelocity();
        Player.Rigidbody.AddForce(Vector2.up * propulsionForce, ForceMode2D.Impulse);
    }
}
