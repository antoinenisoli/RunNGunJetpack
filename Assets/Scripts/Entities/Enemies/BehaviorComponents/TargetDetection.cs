using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class TargetDetection : MonoBehaviour
{
    [SerializeField] LayerMask detectionMask;
    [SerializeField] float escapeDelay = 3f;
    [SerializeField] PointHelper innerDetection, outerDetection;
    Enemy enemy;
    bool paused;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        innerDetection.ShowGizmos();
        outerDetection.ShowGizmos();
    }
#endif

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    IEnumerator ForgetTarget()
    {
        float timer = escapeDelay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        paused = false;
        enemy.SetTarget(null);
    }

    float TargetDistance()
    {
        return Vector2.Distance(enemy.Target.transform.position, transform.position); 
    }

    private void FixedUpdate()
    {
        if (!enemy)
            return;

        if (enemy.Target)
        {
            float dist = TargetDistance();
            if (dist > outerDetection.Radius && !paused)
            {
                paused = true;
                StartCoroutine(ForgetTarget());
            }
        }
        else
        {
            var colliders = innerDetection.OverlapColliders(detectionMask);
            foreach (var item in colliders)
            {
                PlayerController player = item.GetComponentInParent<PlayerController>();
                if (player)
                {
                    paused = false;
                    StopCoroutine(ForgetTarget());
                    enemy.SetTarget(player);
                    break;
                }
            }
        }
    }
}
