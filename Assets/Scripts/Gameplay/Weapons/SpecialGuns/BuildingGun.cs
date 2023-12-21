using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGun : Firearm
{
    [Header(nameof(BuildingGun))]
    [SerializeField] LayerMask interactables;
    [SerializeField] Material shineMaterial;
    [SerializeField] Gradient laserColorGradient;
    [SerializeField] float hookMoveSpeed = 3f;
    [SerializeField] float minDistance = 3f;
    [SerializeField] float moveRange = 25;

    Rigidbody2D hookedObject;
    Rigidbody2D currentTarget;
    Material baseTargetMat;
    LineRenderer lineRenderer;
    Vector2 targetPos;
    float baseGravity;

    public override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void GetWeaponData()
    {
        WeaponData = new WeaponData("Building Gun");
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

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!hookedObject && !IsBlocked())
        {
            RaycastHit2D linecast = Physics2D.Linecast(shootPoint.position, shootPoint.position + shootPoint.right * shootDistance, interactables);
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

                if (Input.GetButtonDown("Fire1"))
                {
                    float distance = Vector2.Distance(linecast.point, shootPoint.position);
                    if (distance > minDistance)
                        OnHook(linecast.rigidbody);
                }
            }
            else
            {
                if (currentTarget)
                    TargetLost();
            }
        }
        else if(currentTarget)
            TargetLost();


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
        Shoot(false);
        hookedObject = other;
        baseGravity = hookedObject.gravityScale;
        hookedObject.gravityScale = 0;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, hookedObject.position);
        targetPos = CameraManager.Instance.MousePosition();
    }

    void MoveObject()
    {
        hookedObject.constraints = RigidbodyConstraints2D.FreezeRotation;
        hookedObject.velocity = CameraManager.Instance.MousePosition() - hookedObject.position;
        hookedObject.velocity *= hookMoveSpeed;
        lineRenderer.SetPosition(0, shootPoint.position);
        lineRenderer.SetPosition(1, hookedObject.position);
        float value = GetDistance() / moveRange;
        lineRenderer.endColor = laserColorGradient.Evaluate(value);

        if (GetDistance() > moveRange)
            StopHook();
    }

    void StopHook()
    {
        hookedObject.gravityScale = baseGravity;
        hookedObject.constraints = RigidbodyConstraints2D.None;
        hookedObject = null;
        lineRenderer.positionCount = 0;
    }

    public override bool Shoot(bool useAmmo = true) => true;
}
