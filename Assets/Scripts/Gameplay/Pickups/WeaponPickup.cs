using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Pickup
{
    protected WeaponData weaponData;

    string GetGunJson()
    {
        return JsonUtility.ToJson(weaponData);
    }

    public override void OnCollisionWithPlayer(PlayerController player, Collider2D collision)
    {
        EventManager.Instance.OnWeaponChoice.Invoke(this, weaponData);
    }

    public override void OnCollisionExitWithPlayer(PlayerController player, Collider2D collision)
    {
        EventManager.Instance.OnLeaveWeaponChoice.Invoke();
    }
}
