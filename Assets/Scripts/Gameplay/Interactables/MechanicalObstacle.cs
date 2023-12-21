using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicalObstacle : Mechanism
{
    [SerializeField] Transform[] columns;

    public override void Unlock()
    {
        foreach (var item in columns)
        {
            
        }
    }
}
