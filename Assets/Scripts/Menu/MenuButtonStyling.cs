using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonStyling : MonoBehaviour
{
    public Texture2D pointer;

    public void OnMouseOver()
    {
        Cursor.SetCursor (pointer, Vector2.zero, CursorMode.Auto);
    }
    public void OnMouseExit()
    {
        Cursor.SetCursor (null,Vector2.zero,CursorMode.Auto);
    }

    public void onMouseDown(GameObject gO) {
        gO.GetComponent<RectTransform>().localScale *= 0.8f;
    }
    public void onMouseUp(GameObject gO) {
        gO.GetComponent<RectTransform>().localScale *= 1.25f;
    }   
}
