using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PointHelper
{
    public string Name;
    public Transform Point;
    public float Radius;
    [SerializeField] Color GizmoColor;
    [SerializeField] bool Raycast;
    Vector2 direction;

    public void ShowGizmos()
    {
        Gizmos.color = GizmoColor;
        if (Raycast)
            Gizmos.DrawRay(Point.position, direction * Radius);
        else
            Gizmos.DrawWireSphere(Point.position, Radius);
    }

    public Collider2D[] OverlapColliders(LayerMask mask, float range = 0)
    {
        if (range > 0)
            return Physics2D.OverlapCircleAll(Point.position, range, mask);
        else
            return Physics2D.OverlapCircleAll(Point.position, Radius, mask);
    }

    public bool OverlapDetect(LayerMask mask)
    {
        var hit = Physics2D.OverlapCircle(Point.position, Radius, mask);
        return hit;
    }

    public RaycastHit2D RaycastDetect(LayerMask mask, Vector2 direction)
    {
        this.direction = direction;
        RaycastHit2D hit = Physics2D.Raycast(Point.position, direction, Radius, mask);
        return hit;
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Detecting,
    Attacking,
}

public abstract class Enemy : Entity
{
    [Header(nameof(Enemy))]
    [SerializeField] EnemyState currentState;
    [SerializeField] protected float speed;
    [SerializeField] protected int contactDamage = 5;
    [SerializeField] protected float contactPush = 1000f;
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected float closeDistance = 3f;
    [SerializeField] protected PointHelper[] helpers;

    Dictionary<string, PointHelper> _helpersCollection = new Dictionary<string, PointHelper>();
    protected Rigidbody2D rb;
    protected Entity target;
    protected bool forgettingTarget;

    public Entity Target => target; 
    public EnemyState CurrentState => currentState; 

    public override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        foreach (var item in helpers)
            _helpersCollection.Add(item.Name, item);
    }

    public virtual void OnDrawGizmos()
    {
        Color gizmoColor;
        gizmoColor = target == null ? Color.green : Color.red;
        gizmoColor.a = 0.3f;
        Gizmos.color = gizmoColor;

        Gizmos.DrawCube(transform.position, new Vector3(2, 2, 2));
    }

    public virtual void OnDrawGizmosSelected()
    {
        foreach (var item in helpers)
            item.ShowGizmos();
    }

    protected bool InAir()
    {
        return Mathf.Abs(rb.velocity.y) > 0.1f;
    }

    protected bool TargetIsClose()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        return distance < closeDistance;
    }

    protected bool TargetInAir()
    {
        float distance = transform.position.y - target.transform.position.y;
        return Mathf.Abs(distance) > 2f;
    }

    protected float XDistanceToTarget()
    {
        float distance = transform.position.x - target.transform.position.x;
        return Mathf.Abs(distance);
    }

    protected void SetXVelocity(Vector2 newVelocity)
    {
        newVelocity.y = rb.velocity.y;
        rb.velocity = newVelocity;
    }

    protected PointHelper GetPointHelper(string name)
    {
        return _helpersCollection[name];
    }

    public void SetState(EnemyState state)
    {
        if (currentState != state)
            currentState = state;
    }

    public void SetTarget(Entity target)
    {
        this.target = target;
        forgettingTarget = false;
        SetState(EnemyState.Chasing);
    }

    public void ForgetTarget(float escapeDelay = 0)
    {
        if (!forgettingTarget)
        {
            forgettingTarget = true;
            StopCoroutine(ForgetTargetCoroutine(escapeDelay));
            StartCoroutine(ForgetTargetCoroutine(escapeDelay));
        }
    }

    IEnumerator ForgetTargetCoroutine(float escapeDelay = 0)
    {
        float timer = escapeDelay;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (forgettingTarget)
            SetTarget(null);
    }

    public bool CanSeeTarget()
    {
        if (!target)
            return false;

        bool blocked = Physics2D.Linecast(transform.position, target.transform.position, groundMask);
        return !blocked;
    }

    public override void Death()
    {
        base.Death();
        VFXManager.Instance.PlayVFX("BloodFX", transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponentInChildren<PlayerController>();   
        if (player)
            PlayerTouched(player);
    }

    void PlayerTouched(PlayerController player)
    {
        print(name + " collided with player!");
        player.TakeDamage(contactDamage);

        /*Vector2 dir = player.transform.position - transform.position;      
        player.Push(contactPush, dir.normalized);*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponentInChildren<PlayerController>();
        if (player)
            PlayerTouched(player);
    }
}
