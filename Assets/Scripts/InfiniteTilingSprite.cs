using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTilingSprite : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer spr;

    private void Update()
    {
        spr.material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}
