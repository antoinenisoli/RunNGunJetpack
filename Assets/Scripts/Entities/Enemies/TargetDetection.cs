using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] LayerMask detectionMask;
    Enemy enemy;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        if (!enemy)
            return;

        Collider2D coll = Physics2D.OverlapCircle(transform.position, radius, detectionMask);
        if (coll)
        {
            PlayerController player = coll.GetComponentInParent<PlayerController>();
            if (player)
                enemy.SetTarget(player);
        }
    }
}
