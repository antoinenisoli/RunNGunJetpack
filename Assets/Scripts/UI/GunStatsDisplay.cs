using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GunStatsDisplay : WeaponStatDisplay
{
    [SerializeField] Text capacityTxt, fireModeTxt;

    public override void SetWeaponData(WeaponData weaponData)
    {
        base.SetWeaponData(weaponData);
        GunData gunData = weaponData as GunData;
        if (gunData != null)
        {
            capacityTxt.text = gunData.MaxAmmoCapacity.ToString();
            fireModeTxt.text = gunData.FireMode.ToString();
        }
    }
}
