using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    public string name = "Idle";
    [Range(0f,0.5f)] public float frameRate = 0.1f;
    public bool loop = true;
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
        mainSprite = sprites[0];
        animDone = false;
        this.animator = animator;
    }

    public void Update()
    {
        if (animDone && !loop)
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
