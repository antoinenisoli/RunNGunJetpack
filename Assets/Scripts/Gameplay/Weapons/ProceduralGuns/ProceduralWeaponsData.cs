using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProceduralWeaponsData), menuName = "ScriptableObjects/Weapons/" + nameof(ProceduralWeaponsData))]
public class ProceduralWeaponsData : ScriptableObject
{
    public GameObject[] bodies, barrels, scopes, stocks;
    public List<WeaponStatNumber> WeaponStats = new List<WeaponStatNumber>();

    public GunPart RandomPart(GameObject[] parts)
    {
        int random = Random.Range(0, parts.Length);
        return parts[random].GetComponent<GunPart>();
    }

    string RandomName()
    {
        string[] names = new string[] {
            "Single-Shot Photon Shooter",
            "Void Fusion Equalizer",
            "Comet Hand Blaster",
            "Cyclone Gatling Disintegrator",
            "Stormfury Laser Sniper",
            "Battlestar Anti-Matter Shooter",
        };

        int random = Random.Range(0, names.Length);
        return names[random];
    }

    FireMode RandomFireMode()
    {
        System.Array array = System.Enum.GetValues(typeof(FireMode));
        int random = Random.Range(0, array.Length - 1); //exclude lasers
        return (FireMode)array.GetValue(random);
    }

    float GetStatValue(string statName, FireMode mode)
    {
        foreach (var item in WeaponStats)
        {
            if (item.StatName == statName)
                return item.GetValue(mode);
        }

        return 0;
    }

    public string GenerateGunName(params GunPart[] parts)
    {
        string name = "";
        foreach (var item in parts)
        {
            if (!string.IsNullOrEmpty(item.GetPartName()))
                name += item.GetPartName() + " ";
        }

        return name.ToUpper();
    }

    public GunData NewData()
    {
        var mode = RandomFireMode();
        GunPart RandomBody = RandomPart(bodies);
        GunPart RandomBarrel = RandomPart(barrels);
        GunPart RandomScope = RandomPart(scopes);
        GunPart RandomStock = RandomPart(stocks);

        string name = GenerateGunName(RandomBarrel, RandomBody);
        GunData gun = new GunData(
        name, 
        (int)GetStatValue("Damages", mode),
        GetStatValue("AttackRate", mode),
        mode,
        (int)GetStatValue("MagazineSize", mode),
        GetStatValue("BulletSpeed", mode)
        );

        gun.SetSprites(RandomBody.GetSprite(),
            RandomBarrel.GetSprite(),
            RandomScope.GetSprite(),
            RandomStock.GetSprite());

        return gun;
    }
}
