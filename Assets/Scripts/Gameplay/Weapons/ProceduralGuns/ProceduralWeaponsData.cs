using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData
{
    public struct VisualData
    {
        public Sprite body, barrel, scope, stock;
    }

    public VisualData visualData;
    public string Name;
    public int Damages, MaxAmmoCapacity, Range;
    public int FireMode;

    public GunData(string name, int damages, int maxAmmoCapacity, int range, int fireMode)
    {
        Name = name;
        Damages = damages;
        MaxAmmoCapacity = maxAmmoCapacity;
        Range = range;
        FireMode = fireMode;
    }

    public void SetSprites(Sprite body, Sprite barrel, Sprite scope, Sprite stock)
    {
        visualData.body = body;
        visualData.barrel = barrel;
        visualData.scope = scope;
        visualData.stock = stock;
    }
}

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

    public GunData NewData()
    {
        var gun = new GunData(
            "Super cool gun",
            Random.Range(1, 10),
            Random.Range(10, 100),
            Random.Range(1, 5),
            Random.Range(1, 3)
            );

        gun.SetSprites(RandomBody(),
            RandomBarrel(),
            RandomScope(),
            RandomStock());

        return gun;
    }
}
