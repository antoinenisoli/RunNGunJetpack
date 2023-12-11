using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    bool canPickupWeapon;
    RandomGunPickup gunPickup;
    GunData gunPickupData;
    ProceduralGunSprite playerGunSprite;

    Weapon[] weapons;
    int index = 0;

    private void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>();
        for (int i = 0; i < weapons.Length; i++)
            weapons[i].gameObject.SetActive(i == index);
    }

    private void Start()
    {
        playerGunSprite = GetComponentInChildren<ProceduralGunSprite>();
        EventManager.Instance.OnWeaponChoice.AddListener(EnterPickupMode);
        EventManager.Instance.OnLeaveWeaponChoice.AddListener(ExitPickupMode);
    }

    void EnterPickupMode(RandomGunPickup gunObj, GunData gunData)
    {
        gunPickup = gunObj;
        gunPickupData = gunData;
        canPickupWeapon = true;
    }

    void ExitPickupMode()
    {
        gunPickup = null;
        gunPickupData = null;
        canPickupWeapon = false;
    }

    private void Update()
    {
        if (canPickupWeapon && gunPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerGunSprite.SetSprites(gunPickupData.visualData);

                Destroy(gunPickup.gameObject);
                EventManager.Instance.OnLeaveWeaponChoice.Invoke();
                canPickupWeapon = false;
                gunPickup = null;
            }
        }

        if (Input.GetButtonDown("SwitchWeapon"))
        {
            index++;
            index %= weapons.Length;
            for (int i = 0; i < weapons.Length; i++)
                weapons[i].gameObject.SetActive(i == index);
        }
    }
}
