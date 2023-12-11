using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGunPickup : Pickup
{
    public ProceduralWeaponsData mainData;
    public ProceduralGunSprite gunSprite;
    GunData gunData;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        gunData = mainData.NewData();
        gunSprite.SetSprites(gunData.visualData);
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
