using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header(nameof(Entity))]
    public Health Health;
    [SerializeField] Material hitMaterial;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] healthSprites;

    Material spriteMaterial;
    Sequence hitSequence;

    public virtual void Awake()
    {
        spriteMaterial = spriteRenderer.material;
        Health.Initialize();
        Health.OnDamageTaken.AddListener(CheckHealth);
    }

    public virtual void TakeDamage(int amount)
    {
        Health.TakeDamage(amount);
        Hit();

        if (Health.isDead)
            Death();
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

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public int GetHealthSprite()
    {
        float health = Health.value;
        for (float i = 1; i < healthSprites.Length + 1; i++)
        {
            float computeIndex = i / (float)healthSprites.Length;
            if (health <= computeIndex)
            {
                int index = (int)i - 1;
                return index;
            }
        }

        return 0;
    }

    private void CheckHealth()
    {
        if (healthSprites.Length > 0)
        {
            int index = GetHealthSprite();
            spriteRenderer.sprite = healthSprites[index];
        }
    }
}
