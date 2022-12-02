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
    public abstract ModuleType myType { get; }
    protected PlayerController Player;

    public virtual void Initialize(PlayerController player)
    {
        Player = player;
    }

    public virtual void Use()
    {

    }
}
