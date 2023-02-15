using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoisonTrail : Trap
{
    [SerializeField] float lifeTime = 1f;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);
        StartCoroutine(Destruction());
    }

    IEnumerator Destruction()
    {
        yield return new WaitForSeconds(lifeTime);
        transform.DOScale(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
