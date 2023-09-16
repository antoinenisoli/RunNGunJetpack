using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : Weapon
{
    [Header(nameof(GravityGun))]
    [SerializeField] LayerMask interactables;
    [SerializeField] float moveSpeed = 12f;
    [SerializeField] float hookRange = 50, moveRange = 75;

    Rigidbody2D hookedObject;
    LineRenderer lineRenderer;
    Vector2 targetPos;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(shootPoint.position, shootPoint.right * hookRange);
    }

    public override void Execute()
    {
        base.Execute();
        Debug.DrawRay(shootPoint.position, shootPoint.right * hookRange);
        if (Input.GetButtonDown("Fire1") && !hookedObject)
        {
            var hit = Physics2D.Raycast(shootPoint.position, shootPoint.right, hookRange, interactables);
            if (hit.transform)
            {
                OnHook(hit.rigidbody);
            }
        }

        if (Input.GetButton("Fire1") && hookedObject)
            MoveObject();

        if (Input.GetButtonUp("Fire1") && hookedObject)
            StopHook();
    }

    float GetDistance()
    {
        return Vector2.Distance(shootPoint.position, hookedObject.position);
    }

    void OnHook(Rigidbody2D other)
    {
        hookedObject = other;
        hookedObject.gravityScale = 0;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, hookedObject.position);
        targetPos = MousePosition();
    }

    void MoveObject()
    {
        hookedObject.velocity = MousePosition() - hookedObject.position;
        hookedObject.velocity *= moveSpeed;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, hookedObject.position);
        if (GetDistance() > moveRange)
            StopHook();
    }

    void StopHook()
    {
        hookedObject.gravityScale = 1;
        hookedObject = null;
        lineRenderer.positionCount = 0;
    }
}
