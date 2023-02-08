using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void FreezeFrame(float timeValue, float duration)
    {
        StopCoroutine(Freeze(timeValue, duration));
        StartCoroutine(Freeze(timeValue, duration));
    }

    IEnumerator Freeze(float timeValue, float duration)
    {
        Set(timeValue);
        yield return new WaitForSecondsRealtime(duration);
        Set(1f);
    }

    public void SlowMotion(float timeValue = 0.3f, float duration = 1f, Ease ease = Ease.Linear)
    {
        DOVirtual.Float(timeValue, 1, duration, Set).SetEase(ease).SetUpdate(true);
    }

    public static void Set(float f)
    {
        Time.timeScale = f;
    }
}
