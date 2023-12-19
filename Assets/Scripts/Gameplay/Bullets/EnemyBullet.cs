using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    //[Header(nameof(EnemyBullet))]

    public override bool CantTakeDamage(Entity entity)
    {
        return entity is Enemy;
    }
}
