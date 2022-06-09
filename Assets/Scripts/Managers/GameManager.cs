using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


    [Tooltip("Player")]
    public GameObject player;
    
    [Tooltip("Canvas")]
    public Canvas _canvas;
    public static bool gameIsPaused = false;

    [Tooltip("menu when pressed ESC")]
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void PauseGame (bool needTriggerPauseMenu = true)
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
            if(needTriggerPauseMenu) pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else 
        {
            Time.timeScale = 1;
            if(needTriggerPauseMenu) pauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void GameOver() {
        SceneManager.LoadScene("GameOver");

    }
}
