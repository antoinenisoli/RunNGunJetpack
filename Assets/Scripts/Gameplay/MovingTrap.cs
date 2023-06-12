using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class MovingTrap : DamageTrap
{
    [Header(nameof(MovingTrap))]
    [SerializeField] float speed = 1f;
    [SerializeField] Transform leftPosition, rightPosition;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(leftPosition.position, rightPosition.position);
    }

    float Pulse()
    {
        return 0.5f * (1 + Mathf.Sin(2 * Mathf.PI * speed * Time.time));
    }

    private void Update()
    {
        float sin = Pulse();
        Vector2 pos = Vector2.Lerp(leftPosition.position, rightPosition.position, sin);
        transform.position = pos;
    }
}
