// CurveAttribute.cs
// Created by Alexander Ameye
// Version 1.1.0

using UnityEngine;

public class CurveAttribute : PropertyAttribute
{
    public float RangeX, RangeY;
    public int x;

    public CurveAttribute(float RangeX, float RangeY)
    {
        this.RangeX = RangeX;
        this.RangeY = RangeY;
    }
}