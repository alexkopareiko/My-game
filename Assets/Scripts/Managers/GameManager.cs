using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("GameOver menu from canvas")]

    public GameObject gameOverPanel;


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

    public void PauseGame ()
    {
        if(gameIsPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else 
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            Cursor.visible = false;
        }
    }

    public void GameOver() {
        gameOverPanel.SetActive(true);
    }
}
