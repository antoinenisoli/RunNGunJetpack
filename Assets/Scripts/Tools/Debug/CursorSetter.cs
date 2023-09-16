using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] Texture2D text;

    private void Awake()
    {
        Cursor.SetCursor(text, new Vector2(text.width/2, text.height/2), CursorMode.Auto);
    }
}
