using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header(nameof(GroundEnemy))]
    [SerializeField] float closeDistanceRange = 10f;
    AnimationScript animScript;
    MeleeWeapon meleeWeapon;

    private void Start()
    {
        animScript = GetComponentInChildren<AnimationScript>();
        meleeWeapon = GetComponentInChildren<MeleeWeapon>();
    }

    void Patrol()
    {
        SetState(EnemyState.Idle);
        animScript.StartAnim("Run");
        SetXVelocity(transform.right * speed);
        if (!InAir() && !GetPointHelper("Corner").OverlapDetect(groundMask))
            transform.Rotate(Vector3.up * 180);
    }

    void Stop()
    {
        SetXVelocity(new Vector2());
        animScript.StartAnim("Idle");
    }

    void ChasePlayer()
    {
        if (!TargetIsClose())
        {
            if (GetPointHelper("Corner").OverlapDetect(groundMask))
            {
                SetXVelocity((target.transform.position - transform.position).normalized * speed);
                if (Mathf.Abs(rb.velocity.x) > 1.5f)
                    animScript.StartAnim("Run");
                else
                    animScript.StartAnim("Idle");
            }
        }

        if (!CanSeeTarget())
            ForgetTarget(3f);
        else if ((TargetIsClose() && !TargetInAir()) || TargetInAir() && XDistanceToTarget() < closeDistanceRange)
        {
            Stop();
            SetState(EnemyState.Attacking);
        }
    }

    void AttackPlayer()
    {
        if (TargetInAir())
            RangeAttack();
        else if (meleeWeapon)
            meleeWeapon.ExecuteTimer();

        if (TargetInAir())
        {
            if (!CanSeeTarget() || XDistanceToTarget() > closeDistanceRange)
                SetState(EnemyState.Chasing);
        }
        else if (!TargetIsClose())
            SetState(EnemyState.Chasing);
    }

    void RangeAttack()
    {
        print("range attack");
    }

    public void MeleeAttackAnim() //called by event
    {
        string[] attackAnimations = new string[] { "Attack1", "Attack2" };
        int random = Random.Range(0, attackAnimations.Length);
        string randomAnimation = attackAnimations[random];
        animScript.StartAnim(randomAnimation);
    }

    private void RotateTowardsTarget()
    {
        if (transform.position.x > target.transform.position.x)
            transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        else
            transform.rotation = Quaternion.Euler(new Vector3());
    }

    private void Update()
    {
        if (target)
        {
            RotateTowardsTarget();
            switch (CurrentState)
            {
                case EnemyState.Chasing:
                    ChasePlayer();
                    break;

                case EnemyState.Detecting:
                    break;

                case EnemyState.Attacking:
                    AttackPlayer();
                    break;

                default:
                    break;
            }
        }
        else
            Patrol();
    }

}
