using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    [Header(nameof(FlyEnemy))]
    [SerializeField] Vector2 additionalVelocity;
    [SerializeField] float shootDistance;
    [SerializeField] GunData gunData;
    Vector2 chaseVelocity;
    Gun myWeapon;

    float GetDistance() => Vector2.Distance(target.transform.position, transform.position);

    public override void Awake()
    {
        base.Awake();
        myWeapon = GetComponentInChildren<Gun>();
        myWeapon.GunData = gunData;
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

    void ChasePlayer()
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
    }

    void AttackPlayer()
    {
        myWeapon.ExecuteTimer();
        myWeapon.LookAt(myWeapon.transform, target.transform.position);
    }

    private void Update()
    {
        ChasePlayer();
        if (target && CanSeeTarget())
            AttackPlayer();

        additionalVelocity = Vector2.Lerp(additionalVelocity, Vector2.zero, 1f * Time.deltaTime);
        rb.velocity = chaseVelocity + additionalVelocity;
    }

    public void AddVelocity(Vector2 vel)
    {
        additionalVelocity = vel;
    }
}
