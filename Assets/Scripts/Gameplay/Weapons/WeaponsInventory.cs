using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInventory : MonoBehaviour
{
    Weapon[] weapons;
    int index = 0;

    private void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>();
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].gameObject.SetActive(i == index);
    }

    private void Update()
    {
        if (Input.GetButtonDown("SwitchWeapon"))
        {
            index++;
            index %= weapons.Length;
            for (int i = 0; i < weapons.Length; i++)
                weapons[i].gameObject.SetActive(i == index);
        }
    }
}
