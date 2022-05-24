using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [Tooltip("Sound of bullet hitting")]
    public AudioClip bulletHittingSound;    
    
    [Tooltip("Sound of headshot")]
    public AudioClip headshotSound;
    
    [Tooltip("Sound of die")]
    public AudioClip dieSound;

    GameManager gameManager;

    [Tooltip("[PRIVATE] How many parts with ConfigurableJoint enemy has")]
    [SerializeField] private int _health;

    [Tooltip("Audio Source Enemy")]
    private AudioSource audioSource;

    private void Start()
    {
        gameManager = GameManager.instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        // find physical part of enemy
        Unit enemyUnit = transform.GetComponentInChildren<Unit>();

        // find all Configurable Joint components
        ConfigurableJoint[] CJcomponents = enemyUnit.gameObject.GetComponentsInChildren<ConfigurableJoint>();
        _health = CJcomponents.Length;

    }

    // return health of enemy
    public int getHealth()
    {
        return _health;
    }

    // hit enemy (-1), returns health
    public int HitEnemy(GameObject block)
    {
        // find and destroy connection with physical part
        ConfigurableJoint enemyCJ = block.GetComponent<ConfigurableJoint>();
        PhysicalBodyPart enemyPBP = block.GetComponent<PhysicalBodyPart>();

        if (block.GetComponent<Head>() && enemyCJ && enemyPBP)
        {
            gameManager.player.GetComponent<Player>().audioSource.PlayOneShot(headshotSound, 1.0f);
        }
        else
        {
            audioSource.PlayOneShot(bulletHittingSound, 1.0f);
        }

        if (enemyCJ && enemyPBP)
        {
            Destroy(enemyPBP);
            Destroy(enemyCJ);

            _health -= 1;
        }
        
        if(_health <= 3)
        {
            Die();
        }
        return _health;
    }

    // destroy enemy
    public void Die()
    {
        Destroy(gameObject);
        gameManager.player.GetComponent<Player>().audioSource.PlayOneShot(dieSound, 1.0f);
    }
}
