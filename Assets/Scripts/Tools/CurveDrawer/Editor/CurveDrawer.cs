// CurveDrawer.cs
// Created by Alexander Ameye
// Version 1.1.0

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CurveAttribute))]
public class CurveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        CurveAttribute curve = attribute as CurveAttribute;
        if (property.propertyType == SerializedPropertyType.AnimationCurve)
        {
            EditorGUI.CurveField(position, property, Color.cyan, new Rect(0, 0, curve.RangeX, curve.RangeY));
        }
    }
}