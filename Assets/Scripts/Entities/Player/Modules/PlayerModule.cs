using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleType
{
    Propulsion,
    Slide,
}

public abstract class PlayerModule : MonoBehaviour
{
    [Header(nameof(PlayerModule))]
    public string buttonName;
    protected PlayerController Player;
    public abstract ModuleType myType { get; }

    public virtual void Initialize(PlayerController player)
    {
        Player = player;
    }

    public virtual void Use()
    {

    }
}
