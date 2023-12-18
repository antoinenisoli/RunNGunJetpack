using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData
{
    public string Name;
    public int Damages;
    public float BulletSpeed;

    public WeaponData(string name, int damages, float bulletSpeed)
    {
        Name = name;
        Damages = damages;
        BulletSpeed = bulletSpeed;
    }
}

public abstract class Weapon : MonoBehaviour
{
    [Header(nameof(Weapon))]
    [SerializeField] protected Transform weaponVisual;
    private WeaponData weaponData;
    Camera cam;

    public virtual WeaponData WeaponData { get => weaponData; set => weaponData = value; }

    public virtual void Awake()
    {
        cam = Camera.main;
    }

    public virtual void Execute() { }

    private void Update()
    {
        Execute();
    }
}
