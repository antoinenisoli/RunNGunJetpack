using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public class GunPickupEvent : UnityEvent<WeaponPickup, WeaponData> { }
    public class WeaponEvent : UnityEvent<WeaponData> { }
    public GunPickupEvent OnWeaponChoice = new GunPickupEvent();
    public UnityEvent OnLeaveWeaponChoice = new UnityEvent();
    public WeaponEvent OnNewGunSelected = new WeaponEvent();

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
