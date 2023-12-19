using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponHUD : MonoBehaviour
{
    [SerializeField] ProceduralGunUI playerGunUI;
    [SerializeField] Text ammoText, weaponNameText;
    [SerializeField] RectTransform[] weaponDisplays;
    WeaponsManager weaponsManager;

    Weapon weapon => weaponsManager.CurrentGun;

    private void Awake()
    {
        weaponsManager = FindObjectOfType<WeaponsManager>();
        if (!weaponsManager)
            Destroy(gameObject);
    }

    private void Start()
    {
        EventManager.Instance.OnNewGunSelected.AddListener(DisplayCurrentGun);
        DisplayCurrentGun(weapon.WeaponData);
    }

    void ShowWeapon(int index)
    {
        for (int i = 0; i < weaponDisplays.Length; i++)
            weaponDisplays[i].gameObject.SetActive(i == index);
    }

    void DisplayCurrentGun(WeaponData data)
    {
        Gun playerGun = weapon as Gun;
        weaponNameText.text = weapon.WeaponData.Name;

        if (playerGun)
        {
            ShowWeapon(0);
            playerGunUI.SetSprites(playerGun.GunData.visualData);
            ammoText.gameObject.SetActive(true);
        }
        else if (weapon)
        {
            ammoText.gameObject.SetActive(false);
            ShowWeapon(1);
        }
    }

    private void Update()
    {
        Gun playerGun = weapon as Gun;
        if (playerGun)
        {
            AmmoSystem ammoSystem = playerGun.AmmoSystem;
            ammoText.text = ammoSystem.AmmoAmount + "";
        }
    }
}
