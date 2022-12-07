using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] string startAnim = "Idle";
    [SerializeField] bool destroyOnEnd;
    [SerializeField] protected SpriteRenderer animRenderer;
    [SerializeField] protected CustomAnimation[] animations = new CustomAnimation[1] { new CustomAnimation("Idle") };

    Dictionary<string, CustomAnimation> customAnimations = new Dictionary<string, CustomAnimation>();
    CustomAnimation currentAnim;

    private void Awake()
    {
        foreach (var item in animations)
        {
            item.Init(this);
            customAnimations.Add(item.name, item);
        }
    }

    private void Start()
    {
        StartAnim(startAnim);
    }

    public void SelfDestroy()
    {
        if (destroyOnEnd)
            Destroy(gameObject);
    }

    public void StartAnim(string name)
    {
        currentAnim = customAnimations[name];
    }

    private void Update()
    {
        if (currentAnim != null)
        {
            currentAnim.Update();
            animRenderer.sprite = currentAnim.mainSprite;
        }
    }
}
