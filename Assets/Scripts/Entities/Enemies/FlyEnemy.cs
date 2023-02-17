using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    [Header(nameof(FlyEnemy))]
    public Vector2 additionalVelocity;
    [SerializeField] float shootDistance;
    Vector2 chaseVelocity;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }
#endif

    public override void Awake()
    {
        base.Awake();
    }

    public bool CanShoot()
    {
        if (!target)
            return false;

        float distance = Vector2.Distance(target.transform.position, transform.position);
        return distance < shootDistance;
    }

    void Deceleration()
    {
        chaseVelocity = Vector2.Lerp(chaseVelocity, Vector2.zero, 5f * Time.deltaTime);
    }

    float GetDistance() => Vector2.Distance(target.transform.position, transform.position);

    private void Update()
    {
        if (target)
        {
            Vector2 followTarget = Target.transform.position - transform.position;

            if (GetDistance() > shootDistance)
                chaseVelocity = followTarget * speed;
            else if (GetDistance() < shootDistance / 2)
                chaseVelocity = -followTarget * speed;
            else
                Deceleration();
        }
        else if (rb.velocity.magnitude > 0.001f)
            Deceleration();

        additionalVelocity = Vector2.Lerp(additionalVelocity, Vector2.zero, 1f * Time.deltaTime);
        rb.velocity = chaseVelocity + additionalVelocity;
    }

    public void AddVelocity(Vector2 vel)
    {
        additionalVelocity = vel;
    }
}
