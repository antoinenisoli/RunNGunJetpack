using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string Name;
    public int Damages;
    public float AttackRate;

    public WeaponData(string name, int damages = 0, float attackRate = 0)
    {
        Name = name;
        Damages = damages;
        AttackRate = attackRate;
    }
}
