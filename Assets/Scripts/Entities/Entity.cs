using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public Health Health;
    [SerializeField] Material hitMaterial;
    [SerializeField] SpriteRenderer spriteRenderer;

    Material spriteMaterial;
    Sequence hitSequence;

    public void Awake()
    {
        spriteMaterial = spriteRenderer.material;
        Health.Initialize();
    }

    public void TakeDamage(int amount)
    {
        Health.CurrentHealth -= amount;
        Hit();
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
