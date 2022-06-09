using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    AudioSource audioSource;

    [Tooltip("Sound of game over")]
    public AudioClip gameoverSound;
    private void Start() {
        audioSource = Camera.main.GetComponent<AudioSource>();
        if(PlayerPrefs.GetInt("sound") == 1)
            audioSource.PlayOneShot(gameoverSound, 1.0f);
    }
    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

}
