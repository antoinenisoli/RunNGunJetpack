using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public class GunPickupEvent : UnityEvent<RandomGunPickup, GunData> { }
    public GunPickupEvent OnWeaponChoice = new GunPickupEvent();
    public UnityEvent OnLeaveWeaponChoice = new UnityEvent();

    private void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
