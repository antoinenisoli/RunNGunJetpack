using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallcrawlerEnemy : Enemy
{
    [System.Serializable]
    struct LayerDetector
    {
        public string Name;
        public float Radius;
        public Transform Detector;
        public Color GizmoColor;
        public bool Raycast;
        Vector2 direction;

        public void ShowGizmos()
        {
            Gizmos.color = GizmoColor;
            if (Raycast)
                Gizmos.DrawRay(Detector.position, direction * Radius);
            else
                Gizmos.DrawWireSphere(Detector.position, Radius);
        }

        public bool OverlapDetect(LayerMask mask)
        {
            var hit = Physics2D.OverlapCircle(Detector.position, Radius, mask);
            return hit;
        }

        public RaycastHit2D RaycastDetect(LayerMask mask, Vector2 direction)
        {
            this.direction = direction;
            RaycastHit2D hit = Physics2D.Raycast(Detector.position, direction, Radius, mask);
            return hit;
        }
    }

    [Header(nameof(WallcrawlerEnemy))]
    [SerializeField] LayerDetector[] detectors;
    Dictionary<string, LayerDetector> _detectorsCollection = new Dictionary<string, LayerDetector>();
    Vector2 lastGroundPos;

    public override void Awake()
    {
        base.Awake();
        foreach (var item in detectors)
            _detectorsCollection.Add(item.Name, item);

        SnapToGround();
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var item in detectors)
            item.ShowGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lastGroundPos, 1f);
    }

    void SnapToGround()
    {
        var hit = Physics2D.Raycast(GetDetector("Ground").Detector.position, -GetDetector("Ground").Detector.up, 1000, groundMask);
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
        var hit = GetDetector("Ground").RaycastDetect(groundMask, -GetDetector("Ground").Detector.up);
        if (hit)
        {
            lastGroundPos = hit.point;
            Snap(transform.rotation.z == 0 || transform.rotation.z == -1);
        }
    }

    LayerDetector GetDetector(string name)
    {
        return _detectorsCollection[name];
    }

    void WalkAlongPath()
    {
        if (GetDetector("Wall").OverlapDetect(groundMask))
        {
            //print("wall");
            transform.Rotate(Vector3.forward * 90);
            return;
        }

        if (!GetDetector("Corner").OverlapDetect(groundMask))
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
