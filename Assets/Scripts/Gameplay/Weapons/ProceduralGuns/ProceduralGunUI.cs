using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralGunUI : MonoBehaviour
{
    public Image bodySpr, barrelSpr, scopeSpr, stockSpr;

    public void SetSprites(Sprite body, Sprite barrel, Sprite scope, Sprite stock)
    {
        bodySpr.sprite = body;
        barrelSpr.sprite = barrel;
        scopeSpr.sprite = scope;
        stockSpr.sprite = stock;
    }

    public void SetSprites(GunData.VisualData visualData)
    {
        bodySpr.sprite = visualData.body;
        barrelSpr.sprite = visualData.barrel;
        scopeSpr.sprite = visualData.scope;
        stockSpr.sprite = visualData.stock;
    }
}
