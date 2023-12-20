using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ScriptableWeaponObject), menuName = "ScriptableObjects/Weapons/" + nameof(ScriptableWeaponObject))]
public class ScriptableWeaponObject : ScriptableObject
{
    [SerializeField] Sprite weaponIcon;
    [SerializeField] GunData GunData;
    [SerializeField] MeleeWeaponData WeaponData;

    public GunData GetGunData()
    {
        return GunData;
    }

    public MeleeWeaponData GetWeaponData()
    {
        return WeaponData;
    }
}
