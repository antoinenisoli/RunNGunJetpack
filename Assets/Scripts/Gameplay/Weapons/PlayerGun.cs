using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    [Header(nameof(PlayerGun))]
    [SerializeField] RandomGunGenerator randomGunGenerator;

    public override void Awake()
    {
        base.Awake();
        SetGunData(randomGunGenerator.Generate());
    }

    public void SetGunData(GunData newData)
    {
        GunData = newData;
        randomGunGenerator.gunSprite.SetSprites(GunData.visualData);
        AmmoSystem.MaxAmmo = newData.MagazineSize;
        AmmoSystem.AmmoAmount = AmmoSystem.MaxAmmo;
    }
}
