using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    public abstract void CollisionEffect(PlayerController player);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInChildren<PlayerController>();
        if (player)
            CollisionEffect(player);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.collider.GetComponentInChildren<PlayerController>();
        if (player)
            CollisionEffect(player);
    }
}
