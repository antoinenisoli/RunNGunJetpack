using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RandomGunStatsDisplay : MonoBehaviour
{
    [SerializeField] float yOffset = 200;
    [SerializeField] RectTransform arrow;
    [SerializeField] Text nameText;
    [SerializeField] Text damagesTxt, capacityTxt, rangeTxt, fireModeTxt;

    CanvasGroup cr;
    RectTransform myTransform;
    RandomGunPickup randomGun;
    WorldCanvasTool canvasTool;

    private void Awake()
    {
        canvasTool = GetComponentInParent<WorldCanvasTool>();
        myTransform = GetComponent<RectTransform>();
        cr = GetComponent<CanvasGroup>();
        cr.alpha = 0;
    }

    private void Start()
    {
        EventManager.Instance.OnWeaponChoice.AddListener(Open);
        EventManager.Instance.OnLeaveWeaponChoice.AddListener(Close);

        Vector3 oldPos = arrow.localPosition;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(arrow.DOLocalMoveY(oldPos.y - 20, 0.5f));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    void Open(RandomGunPickup gunObj, GunData gunData)
    {
        nameText.text = gunData.Name;
        damagesTxt.text = "Damages - " + gunData.Damages.ToString();
        capacityTxt.text = "Ammo capacity - " + gunData.MaxAmmoCapacity.ToString();
        rangeTxt.text = "Range - " + gunData.Range.ToString();
        fireModeTxt.text = "Fire Mode - " + gunData.FireMode.ToString();

        cr.alpha = 1;
        randomGun = gunObj;
    }

    void Close()
    {
        cr.alpha = 0;
        randomGun = null;
    }

    private void Update()
    {
        if (randomGun)
            canvasTool.MoveUIToWorld(myTransform, randomGun.transform, Vector2.up * yOffset);
    }
}
