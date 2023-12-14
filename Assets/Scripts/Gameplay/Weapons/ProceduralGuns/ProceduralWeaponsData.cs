using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProceduralWeaponsData), menuName = "ScriptableObjects/Weapons/" + nameof(ProceduralWeaponsData))]
public class ProceduralWeaponsData : ScriptableObject
{
    public Sprite[] bodies, barrels, scopes, stocks;

    public Sprite RandomSprite(Sprite[] sprites)
    {
        int random = Random.Range(0, sprites.Length);
        return sprites[random];
    }

    string GetRandomNames()
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

    public GunData NewData()
    {
        var gun = new GunData(
            GetRandomNames(),
            Random.Range(1, 5), //dmg
            Random.Range(10, 50), //bullet speed
            Random.Range(35, 100), //magazine size
            Random.Range(0, 2) //fire mode
            );

        gun.SetSprites(RandomSprite(bodies),
            RandomSprite(barrels),
            RandomSprite(scopes),
            RandomSprite(stocks));

        return gun;
    }
}
