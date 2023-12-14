using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    int ammoAmount = 0;
    int maxAmmo;

    public int AmmoAmount 
    { 
        get => ammoAmount; 
        set
        {
            if (value < 0)
                value = 0;
            if (value > MaxAmmo)
                value = MaxAmmo;

            ammoAmount = value;
        }
    }

    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }

    public bool CanShoot() => ammoAmount > 0;
    public bool IsMaxAmmo() => ammoAmount == MaxAmmo;

    public void AddAmmo(int amount)
    {
        AmmoAmount += amount;
    }

    public void UseAmmo()
    {
        AmmoAmount--;
    }
}
