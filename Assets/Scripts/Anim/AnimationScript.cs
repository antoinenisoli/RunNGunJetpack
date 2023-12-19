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
    CustomAnimation previousAnim, currentAnim;
    Queue<CustomAnimation> animQueues = new Queue<CustomAnimation>();

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

    public void StartAnim(CustomAnimation anim)
    {
        if (!anim.loop)
            previousAnim = currentAnim;

        currentAnim = anim;
        anim.Start();
    }

    public void StartAnim(string name)
    {
        CustomAnimation anim = customAnimations[name];
        StartAnim(anim);
    }

    private void Update()
    {
        if (currentAnim != null)
        {
            currentAnim.Update();
            animRenderer.sprite = currentAnim.mainSprite;
            if (currentAnim.Done() && previousAnim != null)
            {
                StartAnim(previousAnim);
                previousAnim = null;
            }
        }
    }
}
