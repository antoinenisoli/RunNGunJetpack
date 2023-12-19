using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    public string name = "Idle";
    [Range(0f,0.5f)] public float frameRate = 0.1f;
    public bool loop = true, destroyObjAtEnd;
    public Sprite mainSprite;
    public Sprite[] sprites;

    bool animDone;
    AnimationScript animator;
    protected float timer;
    protected int spriteIndex;

    public CustomAnimation(string name)
    {
        this.name = name;
        frameRate = 0.1f;
        loop = true;
    }

    public void Init(AnimationScript animator)
    {
        Start();
        this.animator = animator;
    }

    public void Start()
    {
        mainSprite = sprites[0];
        animDone = false;
    }

    public bool Done() => animDone;

    public void Update()
    {
        if (animDone && destroyObjAtEnd)
        {
            animator.SelfDestroy();
            return;
        }

        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            timer = 0;
            spriteIndex++;
            if (spriteIndex >= sprites.Length)
            {
                if (!animDone)
                {
                    animDone = true;
                    if (!loop)
                        return;
                }
            }

            spriteIndex %= sprites.Length;
            mainSprite = sprites[spriteIndex];
        }
    }
}
