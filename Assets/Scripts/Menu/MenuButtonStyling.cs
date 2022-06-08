using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonStyling : MonoBehaviour
{
    public Texture2D pointer;
    public AudioClip menuButtonPressed;
    public AudioClip menuButtonHover;

    private AudioSource audioSource;

    private void Start() {
        audioSource = Camera.main.GetComponent<AudioSource>();
        Cursor.SetCursor (pointer, Vector2.zero, CursorMode.Auto);
    }

    public void OnMouseOver()
    {
        if(PlayerPrefs.GetInt("sound") == 1)
            audioSource.PlayOneShot(menuButtonHover);
    }
    public void OnMouseExit()
    {
    }

    public void onMouseDown(GameObject gO) {
        gO.GetComponent<RectTransform>().localScale *= 0.8f;
        if(PlayerPrefs.GetInt("sound") == 1)
            audioSource.PlayOneShot(menuButtonPressed);
    }
    public void onMouseUp(GameObject gO) {
        gO.GetComponent<RectTransform>().localScale *= 1.25f;
    }   
}
