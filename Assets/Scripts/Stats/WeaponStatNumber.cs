using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponStatNumber : WeaponStat
{
    [Serializable]
    struct FireModeValue
    {
        public FireMode Mode;
        public Vector2 RandomRange;

        public float GetValue(bool Integer)
        {
            float value = UnityEngine.Random.Range(RandomRange.x, RandomRange.y);
            if (Integer)
                return Mathf.Round(value);
            else
                return Mathf.Round(value * 100f) / 100f;
        }
    }

    [SerializeField] bool Integer;
    [SerializeField] FireModeValue[] RandomValue = new FireModeValue[3];

    public float GetValue(FireMode mode)
    {
        foreach (var item in RandomValue)
            if (item.Mode == mode)
                return item.GetValue(Integer);

        return 0;
    }
}
