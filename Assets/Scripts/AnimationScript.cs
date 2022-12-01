using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CustomAnimation
{
    public string name = "Idle";
    public Sprite[] sprites;
    public float frameRate = 0.1f;
    protected float timer;
    protected int spriteIndex;
    public bool loop = true;
    public Sprite mainSprite;

    public void Init()
    {
        mainSprite = sprites[0];
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > frameRate && loop)
        {
            timer = 0;
            spriteIndex++;
            mainSprite = sprites[spriteIndex % sprites.Length];
        }
    }

    public void Stop()
    {
        loop = false;
        timer = 0;
        spriteIndex = 0;
    }
}

public class AnimationScript : MonoBehaviour
{
    [SerializeField] string startAnim = "Idle";
    [SerializeField] protected SpriteRenderer animRenderer;
    [SerializeField] protected CustomAnimation[] animations;
    Dictionary<string, CustomAnimation> customAnimations = new Dictionary<string, CustomAnimation>();
    CustomAnimation currentAnim;

    private void Awake()
    {
        foreach (var item in animations)
        {
            item.Init();
            customAnimations.Add(item.name, item);
        }
    }

    private void Start()
    {
        StartAnim(startAnim);
    }

    void StartAnim(string name)
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
