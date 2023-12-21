using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Balance : MonoBehaviour
{
    [SerializeField] Vector2 limits;
    [SerializeField] LayerMask movableMask;
    [SerializeField] float maxWeight = 100;
    [SerializeField] Transform weightIndicator, goalIndicator;
    BoxCollider2D boxCollider;
    float currentWeight;
    float weightGoal;
    [SerializeField] List<Collider2D> movables = new List<Collider2D>();

    private void Awake()
    {
        weightGoal = Random.Range(maxWeight * 0.25f, maxWeight);
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        var boxOverlap = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.size, 0, movableMask);
        movables = boxOverlap.ToList();

        foreach (var movable in boxOverlap)
        {
            if (!movables.Contains(movable))
                movables.Add(movable);

            BoxCollider2D box = movable as BoxCollider2D;
            var childOverlap = Physics2D.OverlapBoxAll(box.bounds.center, box.size, 0, movableMask);
            foreach (var subMovable in childOverlap)
            {
                if (!movables.Contains(subMovable))
                {
                    print(subMovable.transform);
                    movables.Add(subMovable);
                }
            }
        }
    }

    private void Update()
    {
        ComputeWeight();
        MoveIndicator(weightIndicator, currentWeight / maxWeight);
        MoveIndicator(goalIndicator, weightGoal / maxWeight);
    }

    private void ComputeWeight()
    {
        float weight = 0;
        foreach (var item in movables)
        {
            Movable movable = item.GetComponent<Movable>();
            weight += movable.Weight;
        }

        currentWeight = weight;
    }

    private void MoveIndicator(Transform indicator, float value)
    {
        var posX = Mathf.Lerp(limits.x, limits.y, value);
        Vector2 pos = weightIndicator.localPosition;
        pos.x = posX;
        indicator.localPosition = Vector3.Lerp(indicator.localPosition, pos, 2f * Time.deltaTime);
    }
}
