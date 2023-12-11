using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    protected Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();
        if (player)
            OnCollisionWithPlayer(player, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();
        if (player)
            OnCollisionExitWithPlayer(player, collision);
    }

    public abstract void OnCollisionWithPlayer(PlayerController player, Collider2D collision);
    public virtual void OnCollisionExitWithPlayer(PlayerController player, Collider2D collision) { }
}
