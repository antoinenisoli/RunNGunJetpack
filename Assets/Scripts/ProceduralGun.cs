using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralGun : MonoBehaviour
{
    public Sprite[] bodies, barrels, scopes, stocks;
    public Image bodyImg, barrelImg, scopeImg, stockImg;

    private void Start()
    {
        Generate();
    }

    [ContextMenu(nameof(Generate))]
    public void Generate()
    {
        bodyImg.sprite = RandomSprite(bodies);
        barrelImg.sprite = RandomSprite(barrels);
        scopeImg.sprite = RandomSprite(scopes);
        stockImg.sprite = RandomSprite(stocks);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Generate();
    }

    Sprite RandomSprite(Sprite[] sprites)
    {
        int random = Random.Range(0, sprites.Length);
        return sprites[random];
    }
}
