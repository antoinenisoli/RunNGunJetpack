using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyArea : Trap
{
    public override void CollisionEffect(PlayerController player)
    {
        player.FakeDeath();
    }
}
