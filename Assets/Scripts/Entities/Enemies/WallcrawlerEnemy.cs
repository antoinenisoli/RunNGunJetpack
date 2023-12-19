using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallcrawlerEnemy : Enemy
{
    [Header(nameof(WallcrawlerEnemy))]
    Vector2 lastGroundPos;

    public override void Awake()
    {
        base.Awake();
        SnapToGround();
    }

    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lastGroundPos, 1f);
    }

    void SnapToGround()
    {
        var hit = Physics2D.Raycast(GetPointHelper("Ground").Point.position, -GetPointHelper("Ground").Point.up, 1000, groundMask);
        if (hit)
            transform.position = hit.point;
    }

    void Snap(bool axis)
    {
        //print(axis + " " + transform.rotation.z);
        Vector2 snappedPos = transform.position;
        if (!axis)
            snappedPos.x = lastGroundPos.x;
        else
            snappedPos.y = lastGroundPos.y;

        transform.position = snappedPos;
    }

    void CheckGroundSnapping()
    {
        var hit = GetPointHelper("Ground").RaycastDetect(groundMask, -GetPointHelper("Ground").Point.up);
        if (hit)
        {
            lastGroundPos = hit.point;
            Snap(transform.rotation.z == 0 || transform.rotation.z == -1);
        }
    }

    void WalkAlongPath()
    {
        if (GetPointHelper("Wall").OverlapDetect(groundMask))
        {
            //print("wall");
            transform.Rotate(Vector3.forward * 90);
            return;
        }

        if (!GetPointHelper("Corner").OverlapDetect(groundMask))
        {
            transform.Rotate(Vector3.forward * -90);
            //print("corner");
            return;
        }
    }

    private void Update()
    {
        Vector3 newVelocity = transform.right * speed;
        transform.position += newVelocity * Time.deltaTime;

        CheckGroundSnapping();
        WalkAlongPath();
    }
}
