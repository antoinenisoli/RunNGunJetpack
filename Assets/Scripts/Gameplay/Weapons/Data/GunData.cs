using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunData : WeaponData
{
    public struct VisualData
    {
        public Sprite body, barrel, scope, stock;
    }

    public VisualData visualData = new VisualData();
    public int MagazineSize;
    public float BulletSpeed;
    public FireMode FireMode;

    public GunData(string name, int damages = 0, float attackRate = 0, FireMode fireMode = default, int magazineSize = 0, float bulletSpeed = 0) : base(name, damages, attackRate)
    {
        MagazineSize = magazineSize;
        BulletSpeed = bulletSpeed;
        FireMode = fireMode;
        visualData = new VisualData();
    }

    public void SetSprites(Sprite body, Sprite barrel, Sprite scope, Sprite stock)
    {
        visualData.body = body;
        visualData.barrel = barrel;
        visualData.scope = scope;
        visualData.stock = stock;
    }
}
