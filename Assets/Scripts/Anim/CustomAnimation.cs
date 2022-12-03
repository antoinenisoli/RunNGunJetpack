using UnityEngine;

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
    bool animDone;

    public CustomAnimation(string name)
    {
        this.name = name;
        frameRate = 0.1f;
        loop = true;
    }

    public void Init()
    {
        mainSprite = sprites[0];
        animDone = false;
    }

    public void Update()
    {
        if (animDone && !loop)
            return;

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
