using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header(nameof(Entity))]
    public Health Health;
    [SerializeField] Material hitMaterial;
    [SerializeField] SpriteRenderer spriteRenderer;

    Material spriteMaterial;
    Sequence hitSequence;

    public virtual void Awake()
    {
        spriteMaterial = spriteRenderer.material;
        Health.Initialize();
    }

    public virtual void TakeDamage(int amount)
    {
        Health.CurrentHealth -= amount;
        Hit();

        if (Health.isDead)
            Destroy(gameObject);
    }

    public void Hit()
    {
        if (hitSequence != null)
            hitSequence.Complete();

        hitSequence = DOTween.Sequence();
        hitSequence.AppendCallback(() => { spriteRenderer.material = hitMaterial; });
        hitSequence.AppendInterval(0.05f);
        hitSequence.AppendCallback(() => { spriteRenderer.material = spriteMaterial; });
        hitSequence.Play();
    }
}
