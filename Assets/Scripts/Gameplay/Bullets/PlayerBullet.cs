using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    //[Header(nameof(PlayerBullet))]

    public override bool CantTakeDamage(Entity entity)
    {
        return entity is PlayerController;
    }
}
