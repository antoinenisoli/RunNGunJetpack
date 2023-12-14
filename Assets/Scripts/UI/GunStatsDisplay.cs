using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GunStatsDisplay : WeaponStatDisplay
{
    [SerializeField] Text magazineSizeTxt, fireModeTxt;

    public override void SetWeaponData(WeaponData weaponData)
    {
        base.SetWeaponData(weaponData);
        GunData gunData = weaponData as GunData;
        if (gunData != null)
        {
            magazineSizeTxt.text = gunData.MagazineSize.ToString();
            fireModeTxt.text = gunData.FireMode.ToStringValue();
        }
    }
}
