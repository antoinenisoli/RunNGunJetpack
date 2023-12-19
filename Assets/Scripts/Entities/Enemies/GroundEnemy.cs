using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header(nameof(GroundEnemy))]
    [SerializeField] float closeDistance = 3f;
    [SerializeField] float attackCooldown = 2f;
    AnimationScript animScript;
    float timer;

    private void Start()
    {
        animScript = GetComponentInChildren<AnimationScript>();
    }

    bool InAir()
    {
        return rb.velocity.y > 0.1f || rb.velocity.y < -0.1f;
    }

    void SetXVelocity(Vector2 newVelocity)
    {
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }

    void Patrol()
    {
        currentState = EnemyState.Idle;
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
        if (transform.position.x > target.transform.position.x)
            transform.rotation = Quaternion.Euler(Vector3.up * 180f);
        else
            transform.rotation = Quaternion.Euler(new Vector3());

        float distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > closeDistance)
        {
            if (GetPointHelper("Corner").OverlapDetect(groundMask))
            {
                SetXVelocity((target.transform.position - transform.position).normalized * speed);
                currentState = EnemyState.Chasing;
            }
            else
                Stop();
        }
        else
        {
            if (currentState != EnemyState.Attacking)
            {
                Stop();
                currentState = EnemyState.Attacking;
            }
        }
    }

    void Attack()
    {
        animScript.StartAnim("Attack1");
        print("attack");
        var hits = GetPointHelper("Attack").OverlapColliders(~0);
        foreach (var item in hits)
        {
            Entity entity = item.GetComponentInChildren<Entity>();
            if (entity)
            {
                if (entity is Enemy)
                    continue;

                entity.TakeDamage(10);
            }
        }
    }

    private void Update()
    {
        if (target)
        {         
            if (currentState != EnemyState.Attacking)
                ChasePlayer();
            else
            {
                timer += Time.deltaTime;
                if (timer > attackCooldown)
                {
                    timer = 0;
                    Attack();
                }
            }
        }
        else
            Patrol();
    }
}
