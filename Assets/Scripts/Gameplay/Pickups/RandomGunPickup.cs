using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGunPickup : WeaponPickup
{
    [SerializeField] RandomGunGenerator randomGunGenerator;
    GunData gunData;

    private void Start()
    {
        gunData = randomGunGenerator.Generate();
        weaponData = gunData;
    }

    string GetGunJson()
    {
        return JsonUtility.ToJson(gunData);
    }

    public override void OnCollisionWithPlayer(PlayerController player, Collider2D collision)
    {
        EventManager.Instance.OnWeaponChoice.Invoke(this, gunData);
    }

    public override void OnCollisionExitWithPlayer(PlayerController player, Collider2D collision)
    {
        EventManager.Instance.OnLeaveWeaponChoice.Invoke();
    }
}
