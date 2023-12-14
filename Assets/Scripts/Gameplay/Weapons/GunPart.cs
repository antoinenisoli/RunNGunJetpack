using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunPartType
{
    Barrel,
    Body,
    Scope,
    Stock,
}

public class GunPart : MonoBehaviour
{
    [SerializeField] GunPartType partType;
    [SerializeField] WeaponStatNumber rawStat;
    SpriteRenderer spr;

    public Sprite GetSprite()
    {
        if (!spr)
            spr = GetComponent<SpriteRenderer>();

        return spr.sprite;
    }

    private void Awake()
    {
        if (!spr)
            spr = GetComponent<SpriteRenderer>();
    }

    public string GetPartName()
    {
        if (partType == GunPartType.Barrel || partType == GunPartType.Body)
        {
            string[] split = gameObject.name.Split('_');
            return split[2];
        }

        return null;
    }
}
