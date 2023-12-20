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
    protected float timer;
    protected int spriteIndex = 0;

    public CustomAnimation(string name)
    {
        this.name = name;
        frameRate = 0.1f;
        loop = true;
    }

    public void Start()
    {
        spriteIndex = 0;
        mainSprite = sprites[0];
        animDone = false;
    }

    public bool Done() => animDone;

    public void Update()
    {
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
