using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ProceduralWeaponsData), menuName = "ScriptableObjects/Weapons/" + nameof(ProceduralWeaponsData))]
public class ProceduralWeaponsData : ScriptableObject
{
    public Sprite[] bodies, barrels, scopes, stocks;

    public Sprite RandomBody() => RandomSprite(bodies);
    public Sprite RandomBarrel() => RandomSprite(barrels);
    public Sprite RandomScope() => RandomSprite(scopes);
    public Sprite RandomStock() => RandomSprite(stocks);

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
            Random.Range(1, 10),
            Random.Range(1, 5),
            Random.Range(10, 100),
            Random.Range(1, 3)
            );

        gun.SetSprites(RandomBody(),
            RandomBarrel(),
            RandomScope(),
            RandomStock());

        return gun;
    }
}
