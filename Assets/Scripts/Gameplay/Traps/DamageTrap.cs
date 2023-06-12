using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrap : Trap
{
    [Header(nameof(DamageTrap))]
    [SerializeField] protected int contactDamage = 5;
    [SerializeField] protected float contactPush = 1000f;

    public override void CollisionEffect(PlayerController player)
    {
        //Vector2 dir = player.transform.position - transform.position;
        player.TakeDamage(contactDamage);
        player.Push(contactPush, player.transform.up);
    }
}
