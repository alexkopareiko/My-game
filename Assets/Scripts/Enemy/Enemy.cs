using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Sound of bullet hitting")]
    public AudioClip bulletHittingSound;    
    
    [Tooltip("Sound of headshot")]
    public AudioClip headshotSound;
    
    [Tooltip("Sound of attack")]
    public AudioClip attackSound;
    
    [Tooltip("Sound of die")]
    public AudioClip dieSound;
    
    [Tooltip("Radius of Attack")]
    public float attackRadius = 2;
    
    [Tooltip("Power of Damage")]
    public float powerOfDamage = 3;
    
    [Tooltip("Delay between atacks (seconds)")]
    public float attackDelay = 1;

    [Tooltip("Has attacked")]
    private bool hasAttacked = false;

    private GameManager gameManager;

    [Tooltip("[PRIVATE] How many parts with ConfigurableJoint enemy has")]
    [SerializeField] private int _health;

    [Tooltip("Audio Source Enemy")]
    private AudioSource audioSource;

    [Tooltip("Physical bodyparts")]
    private GameObject physicalBodyParts;

    [Tooltip("Animated bodyparts")]
    private Animated animatedBodyParts;

    private void Start()
    {
        gameManager = GameManager.instance;

        physicalBodyParts = transform.GetComponentInChildren<Unit>().gameObject;
        animatedBodyParts = transform.GetComponentInChildren<Animated>();
        audioSource = physicalBodyParts.GetComponent<AudioSource>();

        // find all Configurable Joint components
        ConfigurableJoint[] CJcomponents = physicalBodyParts.gameObject.GetComponentsInChildren<ConfigurableJoint>();
        _health = CJcomponents.Length;
    }

 

    private void Update()
    {
        CheckForAttack();
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

    // check for attack
    void CheckForAttack()
    {
        GameObject player = gameManager.player;

        if(physicalBodyParts && player)
        {
            float distance = Vector3.Distance(physicalBodyParts.transform.position, player.transform.position);
            //Debug.Log("distance " + distance);
            if (distance <= attackRadius && !hasAttacked)
            {
                hasAttacked = true;
                StartCoroutine(ResetAttacked(attackDelay));
                animatedBodyParts.GetComponent<Animator>().SetTrigger("Attack_Trig");
            }
        }

    }

    IEnumerator ResetAttacked(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject player = gameManager.player;
        float distance = Vector3.Distance(physicalBodyParts.transform.position, player.transform.position);
        if (distance <= attackRadius)
        {
            player.GetComponent<Player>().hitPlayer(powerOfDamage);
            audioSource.PlayOneShot(attackSound, 1.0f);

        }
        hasAttacked = false;
    }

    void OnDrawGizmosSelected()
    {
        // draw bottom sphere of enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(physicalBodyParts.transform.position, 1);

    }


}
