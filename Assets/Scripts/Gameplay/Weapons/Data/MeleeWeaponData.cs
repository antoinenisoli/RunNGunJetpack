using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeWeaponData : WeaponData
{
    public float Range;

    public MeleeWeaponData(string name, int damages = 0, float attackRate = 0, float range = 0) : base(name, damages, attackRate)
    {
        Range = range;
    }
}
