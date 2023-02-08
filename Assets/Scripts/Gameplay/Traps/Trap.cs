using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header(nameof(Trap))]
    [SerializeField] protected int contactDamage = 5;
    [SerializeField] protected float contactPush = 1000f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInChildren<PlayerController>();

        if (player)
        {
            //Vector2 dir = player.transform.position - transform.position;
            player.TakeDamage(contactDamage);
            player.Push(contactPush, player.transform.up);
        }
    }
}
