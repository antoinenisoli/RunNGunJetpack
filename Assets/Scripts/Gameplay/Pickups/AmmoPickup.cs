using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup
{
    [SerializeField] int value;

    public override void OnCollisionWithPlayer(PlayerController player, Collider2D collision)
    {
        AmmoSystem ammoSystem = player.GetComponentInChildren<AmmoSystem>();
        if (!ammoSystem)
            return;

        if (!ammoSystem.IsMaxAmmo())
        {
            ammoSystem.AddAmmo(value);
            Destroy(gameObject);
        }
    }
}
