using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickupDisplay : MonoBehaviour
{
    [SerializeField] float yOffset = 200;
    [SerializeField] RectTransform arrow;
    [SerializeField] GunStatsDisplay currentWeapon, pickupWeapon;

    WeaponsManager weaponsManager;
    CanvasGroup cr;
    RectTransform myTransform;
    WorldCanvasTool canvasTool;
    Transform weaponTransform;

    private void Awake()
    {
        weaponsManager = FindObjectOfType<WeaponsManager>();
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

    void Open(WeaponPickup gunObj, WeaponData weaponData)
    {
        currentWeapon.gameObject.SetActive(weaponsManager.CurrentGun is PlayerGun);
        currentWeapon.SetWeaponData(weaponsManager.CurrentGun.WeaponData);
        pickupWeapon.SetWeaponData(weaponData);

        cr.alpha = 1;
        weaponTransform = gunObj.transform;
    }

    void Close()
    {
        cr.alpha = 0;
        weaponTransform = null;
    }

    private void Update()
    {
        if (weaponTransform)
            canvasTool.MoveUIToWorld(myTransform, weaponTransform, Vector2.up * yOffset);
    }
}
