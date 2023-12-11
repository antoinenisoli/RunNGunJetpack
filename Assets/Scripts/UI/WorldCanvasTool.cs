using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasTool : MonoBehaviour
{
    Canvas canvas;
    RectTransform canvasRect;
    Camera mainCam;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        mainCam = Camera.main;
        canvas.worldCamera = mainCam;
    }

    public void MoveUIToWorld(RectTransform uiObject, Transform worldTarget, Vector2 offset = new Vector2())
    {
        Vector2 ViewportPosition = mainCam.WorldToViewportPoint(worldTarget.position);
        Vector2 WorldObject_ScreenPosition = new Vector2
            (
        (ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
        (ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
        );

        uiObject.anchoredPosition = WorldObject_ScreenPosition + offset;
    }
}
