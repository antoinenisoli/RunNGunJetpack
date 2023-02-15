using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : Enemy
{
    [Header(nameof(FlyEnemy))]
    [SerializeField] float shootDistance;
    [SerializeField] float minDistanceWithOthers = 5f;
    FlyEnemy[] otherEnemies;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceWithOthers);
    }
#endif

    public override void Awake()
    {
        base.Awake();
        otherEnemies = FindObjectsOfType<FlyEnemy>(false);
    }

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
            //rb.AddForce(followTarget);
        }
        else if (rb.velocity.magnitude > 0.1f)
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 5f * Time.deltaTime);
    }

    private void ManageRepelling()
    {
        foreach (var item in otherEnemies)
        {
            if (!item)
                continue;

            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < minDistanceWithOthers)
            {
                Vector2 v = transform.position - item.transform.position;
                rb.AddForce(v.normalized * 10f);
            }
        }
    }

    private void FixedUpdate()
    {
        ManageRepelling();
    }
}
