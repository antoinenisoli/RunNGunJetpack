using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAwareness : MonoBehaviour
{
    [SerializeField] float minDistanceWithOthers = 5f;
    FlyEnemy[] otherEnemies;
    List<FlyEnemy> tooCloseEnemies = new List<FlyEnemy>();

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
                //rb.AddForce(v.normalized * 10f);
            }
        }
        /*else
            rb.velocity = new Vector2();*/
    }
}
