using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RandomGunGenerator
{
    public ProceduralWeaponsData mainData;
    public ProceduralGunSprite gunSprite;

    public GunData Generate()
    {
        GunData gunData = mainData.NewData();
        gunSprite.SetSprites(gunData.visualData);
        return gunData;
    }
}
