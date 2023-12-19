using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailSpawner : MonoBehaviour
{
    [SerializeField] GameObject trail;
    [SerializeField] float spawnTrailRate = 0.3f;
    [SerializeField] LayerMask groundMask;
    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnTrailRate)
        {
            timer = 0;
            var groundDetected = Physics2D.OverlapCircle(transform.position, 0.1f, groundMask);
            if (groundDetected)
            {
                GameObject trailSpawned = Instantiate(trail, groundDetected.ClosestPoint(transform.position), transform.rotation);
            }
        }
    }
}
