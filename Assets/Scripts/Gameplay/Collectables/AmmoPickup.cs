using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] int value;

    public override void Effect(Collider2D collision)
    {
        AmmoSystem ammoSystem = collision.GetComponentInChildren<AmmoSystem>();
        if (!ammoSystem)
            return;

        if (!ammoSystem.IsMaxAmmo())
        {
            ammoSystem.AddAmmo(value);
            Destroy(gameObject);
        }
    }
}
