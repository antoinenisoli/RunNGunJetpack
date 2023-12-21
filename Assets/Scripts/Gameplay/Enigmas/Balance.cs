using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Balance : Enigma
{
    [Header(nameof(Balance))]
    [SerializeField] Vector2 limits;
    [SerializeField] LayerMask movableMask;
    [SerializeField] float maxWeight = 100;
    [SerializeField] float goalThreshold = 10f;
    [SerializeField] Transform weightIndicator;
    [SerializeField] Transform[] goalIndicators;
    List<Collider2D> movables = new List<Collider2D>();
    BoxCollider2D boxCollider;
    float currentWeight;
    float weightGoal;

    private void Awake()
    {
        weightGoal = Random.Range(maxWeight * 0.25f, maxWeight);
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        GetTouchingColliders();
    }

    List<Collider2D> GetColliders(Collider2D subMovable)
    {
        List<Collider2D> list = new List<Collider2D>();
        Movable movable = subMovable.GetComponent<Movable>();
        if (movable)
            list.AddRange(movable.GetNeighbours());

        return list;
    }

    void GetTouchingColliders()
    {
        var overlap = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.size, 0, movableMask);
        List<Collider2D> list = overlap.ToList();
        foreach (var subMovable in overlap)
        {
            var oui = GetColliders(subMovable);
            list.AddRange(oui);
        }

        movables = list.Distinct().ToList();
    }

    private void Update()
    {
        ComputeWeight();
        MoveIndicator(weightIndicator, currentWeight / maxWeight);
        MoveIndicator(goalIndicators[0], (weightGoal - goalThreshold) / maxWeight);
        MoveIndicator(goalIndicators[1], (weightGoal + goalThreshold) / maxWeight);
    }

    public override bool Completed()
    {
        return currentWeight > (weightGoal - goalThreshold) && currentWeight < (weightGoal + goalThreshold);
    }

    private void ComputeWeight()
    {
        if (completed)
            return;

        float weight = 0;
        foreach (var item in movables)
        {
            Movable movable = item.GetComponent<Movable>();
            if (movable)
                weight += movable.Weight;
        }

        currentWeight = weight;
        if (Completed())
            CompleteEnigma();
    }

    private void MoveIndicator(Transform indicator, float value)
    {
        var posX = Mathf.Lerp(limits.x, limits.y, value);
        Vector2 pos = new Vector2(posX, indicator.localPosition.y);
        indicator.localPosition = Vector3.Lerp(indicator.localPosition, pos, 2f * Time.deltaTime);
    }
}
