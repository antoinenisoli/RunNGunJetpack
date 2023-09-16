using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    [SerializeField] int ammoAmount, maxAmmo;

    public int AmmoAmount 
    { 
        get => ammoAmount; 
        set
        {
            if (value < 0)
                value = 0;
            if (value > maxAmmo)
                value = maxAmmo;

            ammoAmount = value;
        }
    }

    public bool CanShoot() => ammoAmount > 0;
    public bool IsMaxAmmo() => ammoAmount == maxAmmo;

    public void AddAmmo(int amount)
    {
        AmmoAmount += amount;
    }

    public void UseAmmo()
    {
        AmmoAmount--;
    }
}
