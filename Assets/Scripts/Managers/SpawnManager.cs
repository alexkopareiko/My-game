using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{

    #region Singleton
    public static SpawnManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Tooltip("Enemy prefab to spawn")]
    public GameObject enemyPrefab;
    
    [Tooltip("Pointer Icon Prefab (red triangle)")]
    public GameObject pointerIconPrefab;
    
    [Tooltip("Range X for enemy spawning")]
    public float xRange = 5f;
    
    [Tooltip("Range Z for enemy spawning")]
    public float zRange = 5f;
    
    [Tooltip("Initial count of enemies")]
    public int enemyCount = 1;

    [Tooltip("Sound of New Level")]
    public AudioClip newLevelSound;

    [Tooltip("Text for level")]
    public TMP_Text levelText;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount = PlayerPrefs.GetInt("level_to_load");
        gameManager = GameManager.instance;
        SpawnEnemies(enemyCount);  
        levelText.text = enemyCount.ToString();
        levelText.GetComponentInParent<Animator>().SetTrigger("new_level");
    }

    private void Update()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length == 0)
        {
            SpawnEnemies(++enemyCount);
            int max_score = PlayerPrefs.GetInt("max_score");
            if(max_score < enemyCount && enemyCount - max_score == 1) 
                PlayerPrefs.SetInt("max_score", enemyCount);
            if(PlayerPrefs.GetInt("sound") == 1)
                gameManager.player.GetComponent<Player>().audioSource.PlayOneShot(newLevelSound, 1.0f);
            levelText.text = enemyCount.ToString();
            levelText.GetComponentInParent<Animator>().SetTrigger("new_level");
        }
    }

    // Spawn Enemies
    public void SpawnEnemies(int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            float x = transform.position.x + Random.Range(-xRange, xRange);
            float z = transform.position.z + Random.Range(-zRange, zRange); 

            Vector3 enemyPosition = new Vector3(x, transform.position.y, z);
            // spawn enemy couple of animated and physical parts
            GameObject newEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation);

            // find Enemy Physical part and set a target to it
            Unit enemyUnit = newEnemy.GetComponentInChildren<Unit>();
            enemyUnit._target = gameManager.player.transform; 

            // find Animated part
            Animated animatedPart = newEnemy.GetComponentInChildren<Animated>();

            // find all renderer components
            Renderer[] rendrerParts = animatedPart.gameObject.GetComponentsInChildren<Renderer>();

            // disable render of animated part 
            GameHelpers.DeactivateRenderers(rendrerParts);
        }
    }
}
