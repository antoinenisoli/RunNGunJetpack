using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatDisplay : MonoBehaviour
{
    [SerializeField] protected Text nameText;
    [SerializeField] protected Text damagesTxt, rangeTxt;

    public virtual void SetWeaponData(WeaponData weaponData)
    {
        nameText.text = weaponData.Name;
        damagesTxt.text = weaponData.Damages.ToString();
        rangeTxt.text = weaponData.Range.ToString();
    }
}
