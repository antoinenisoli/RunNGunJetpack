using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    bool canPickupWeapon;
    WeaponPickup weaponPickup;
    WeaponData weaponPickupData;

    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    Weapon currentGun;
    int index = 0;

    PlayerGun mainPlayerGun => weapons[0] as PlayerGun;
    public Weapon CurrentGun { get => currentGun; 
        set
        {
            currentGun = value;
            EventManager.Instance.OnNewGunSelected.Invoke(currentGun.WeaponData);
        }
    }

    private void Awake()
    {
        weapons = GetComponentsInChildren<Weapon>().ToList();
        for (int i = 0; i < weapons.Count; i++)
            weapons[i].gameObject.SetActive(i == index);

        CurrentGun = weapons[0];
    }

    private void Start()
    {
        EventManager.Instance.OnWeaponChoice.AddListener(EnterPickupMode);
        EventManager.Instance.OnLeaveWeaponChoice.AddListener(ExitPickupMode);
    }

    void EnterPickupMode(WeaponPickup gunObj, WeaponData weaponData)
    {
        weaponPickup = gunObj;
        weaponPickupData = weaponData;
        canPickupWeapon = true;
    }

    void ExitPickupMode()
    {
        weaponPickup = null;
        weaponPickupData = null;
        canPickupWeapon = false;
    }

    void PickupWeapon()
    {
        mainPlayerGun.SetGunData(weaponPickupData as GunData);
        Destroy(weaponPickup.gameObject);
        EventManager.Instance.OnNewGunSelected.Invoke(weaponPickupData as GunData);
        EventManager.Instance.OnLeaveWeaponChoice.Invoke();
        canPickupWeapon = false;
        weaponPickup = null;
    }

    void SwitchWeapon()
    {
        index++;
        index %= weapons.Count;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == index)
            {
                weapons[i].gameObject.SetActive(true);
                CurrentGun = weapons[i];
            }
            else
                weapons[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canPickupWeapon && weaponPickup)
        {
            if (Input.GetButtonDown("PickupWeapon"))
                PickupWeapon();
        }

        if (Input.GetButtonDown("SwitchWeapon"))
            SwitchWeapon();
    }
}
