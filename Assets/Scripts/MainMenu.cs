using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Tooltip("1 - on, 0 - off")]
    public int sound = 1;

    public Image soundButton;

    public Sprite soundButtonSpriteOn;
    public Sprite soundButtonSpriteOff;

    public AudioClip soundTheme;

    private AudioSource audioSource;

    private void Start() {
        sound = PlayerPrefs.GetInt("sound");
        SetSoundImage();
    }

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void TriggerSound() {
        if(sound == 0) {
            sound = 1;
        }
        else if(sound == 1) {
            sound = 0;
        }
        PlayerPrefs.SetInt("sound", sound);
        SetSoundImage();
    }

    public void SetSoundImage() {
        if(sound == 0) {
            soundButton.sprite = soundButtonSpriteOff;
        }
        else if(sound == 1) {
            soundButton.sprite = soundButtonSpriteOn;

        }
    }
}

