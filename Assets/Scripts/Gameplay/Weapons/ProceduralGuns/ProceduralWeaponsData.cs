using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProceduralWeaponsData), menuName = "ScriptableObjects/Weapons/" + nameof(ProceduralWeaponsData))]
public class ProceduralWeaponsData : ScriptableObject
{
    public Sprite[] bodies, barrels, scopes, stocks;
    public List<WeaponStatNumber> WeaponStats = new List<WeaponStatNumber>();

    public Sprite RandomSprite(Sprite[] sprites)
    {
        int random = Random.Range(0, sprites.Length);
        return sprites[random];
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
        int random = Random.Range(0, array.Length);
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

    public GunData NewData()
    {
        var mode = RandomFireMode();
        string name = RandomName();
        GunData gun = new GunData(
        name, 
        mode,
        (int)GetStatValue("Damages", mode),
        GetStatValue("BulletSpeed", mode),
        (int)GetStatValue("MagazineSize", mode),
        GetStatValue("FireRate", mode)
        );

        gun.SetSprites(RandomSprite(bodies),
            RandomSprite(barrels),
            RandomSprite(scopes),
            RandomSprite(stocks));

        return gun;
    }
}
