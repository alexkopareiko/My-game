using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Tooltip("Enemy prefab to spawn")]
    public GameObject enemyPrefab;
    
    [Tooltip("Range X for enemy spawning")]
    public float xRange = 5f;
    
    [Tooltip("Range Z for enemy spawning")]
    public float zRange = 5f;
    
    [Tooltip("Initial count of enemies")]
    public int enemyCount = 1;

    GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameManager.instance;
        SpawnEnemies(enemyCount);  
    }

    // Spawn Enemies
    public void SpawnEnemies(int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            float x = Random.Range(-xRange, xRange);
            float z = Random.Range(-zRange, zRange);

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
