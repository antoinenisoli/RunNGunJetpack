using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Feedbacks
{
    public static void ScreenShake(float duration, float strength = 3, int vibrato = 10)
    {
        Camera.main.DOShakePosition(duration, strength, vibrato).SetUpdate(true);
    }

    public static void FreezeFrame(float timeValue = 0.3f, float duration = 1f)
    {
        DOVirtual.Float(timeValue, 1, duration, Set).SetUpdate(true);
    }

    public static void Set(float f)
    {
        Time.timeScale = f;
    }
}
