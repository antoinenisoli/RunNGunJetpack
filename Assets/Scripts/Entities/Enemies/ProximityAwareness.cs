using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(FlyEnemy))]
public class ProximityAwareness : MonoBehaviour
{
    [SerializeField] float minDistanceWithOthers = 5f;
    FlyEnemy myEnemy;
    FlyEnemy[] otherEnemies;
    List<FlyEnemy> tooCloseEnemies = new List<FlyEnemy>();

    private void Start()
    {
        myEnemy = GetComponent<FlyEnemy>();
        otherEnemies = FindObjectsOfType<FlyEnemy>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceWithOthers);
    }
#endif

    private void ManageRepelling()
    {
        foreach (var item in otherEnemies)
        {
            if (!item)
                continue;

            float dist = Vector2.Distance(transform.position, item.transform.position);
            if (dist < minDistanceWithOthers)
            {
                if (!tooCloseEnemies.Contains(item))
                    tooCloseEnemies.Add(item);
            }
            else
            {
                if (tooCloseEnemies.Contains(item))
                    tooCloseEnemies.Remove(item);
            }
        }

        if (tooCloseEnemies.Count > 0)
        {
            foreach (var item in tooCloseEnemies)
            {
                if (!item)
                    continue;

                Vector2 v = transform.position - item.transform.position;
                myEnemy.AddVelocity(v.normalized * 1f);
                //myEnemy.AddVelocity(v);
            }
        }
    }

    private void Update()
    {
        ManageRepelling();
    }
}
