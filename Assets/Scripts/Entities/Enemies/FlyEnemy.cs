using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    [Header(nameof(GroundEnemy))]
    [SerializeField] float shootDistance;

    public bool CanShoot()
    {
        if (!target)
            return false;

        float distance = Vector2.Distance(target.transform.position, transform.position);
        return distance < shootDistance;
    }

    private void Update()
    {
        if (target && !CanShoot())
        {
            Vector2 followTarget = Target.transform.position - transform.position;
            rb.velocity = followTarget * speed;
        }
        else if (rb.velocity.magnitude > 0.1f)
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 5f * Time.deltaTime);
    }
}
