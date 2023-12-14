using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGun : Firearm
{
    [Header(nameof(BuildingGun))]
    [SerializeField] LayerMask interactables;
    [SerializeField] Material shineMaterial;
    [SerializeField] float moveSpeed = 12f;
    [SerializeField] float hookRange = 25, moveRange = 20;

    Rigidbody2D hookedObject;
    Rigidbody2D currentTarget;
    Material baseTargetMat;
    LineRenderer lineRenderer;
    Vector2 targetPos;
    float baseGravity;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        WeaponData = new WeaponData("Building Gun", 0, (int)hookRange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(shootPoint.position, shootPoint.right * hookRange);
    }

    void NewTargetFound(Rigidbody2D target)
    {
        currentTarget = target;
        var renderer = target.GetComponentInChildren<SpriteRenderer>();
        baseTargetMat = renderer.material;
        renderer.material = shineMaterial;
    }

    void TargetLost()
    {
        var renderer = currentTarget.GetComponentInChildren<SpriteRenderer>();
        renderer.material = baseTargetMat;
        currentTarget = null;
    }

    public override void Execute()
    {
        base.Execute();

        if (!hookedObject)
        {
            RaycastHit2D linecast = Physics2D.Linecast(shootPoint.position, shootPoint.position + shootPoint.right * hookRange, interactables);
            if (linecast && linecast.rigidbody)
            {
                Debug.DrawLine(shootPoint.position, linecast.point, Color.blue);
                if (!currentTarget)
                    NewTargetFound(linecast.rigidbody);
                else if (currentTarget != linecast.rigidbody)
                {
                    TargetLost();
                    NewTargetFound(linecast.rigidbody);
                }
            }
            else
            {
                Debug.DrawLine(shootPoint.position, shootPoint.position + shootPoint.right * hookRange, Color.red);
                if (currentTarget)
                    TargetLost();
            }
        }

        if (Input.GetButtonDown("Fire1") && !hookedObject)
        {
            var hit = Physics2D.Raycast(shootPoint.position, shootPoint.right, hookRange, interactables);
            if (hit.transform)
                OnHook(hit.rigidbody);
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
        baseGravity = hookedObject.gravityScale;
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
        hookedObject.gravityScale = baseGravity;
        hookedObject = null;
        lineRenderer.positionCount = 0;
    }
}
