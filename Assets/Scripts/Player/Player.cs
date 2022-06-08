using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [Tooltip("Audio Source Player")]
    public AudioSource audioSource;

    [Tooltip("Health Bar (On Canvas)")]
    public HealthBar healthBar;

    [SerializeField] private float health = 10;

    [Tooltip("Sound of heal")]
    public AudioClip healSound;

    [Tooltip("Sound of game over")]
    public AudioClip gameoverSound;

    private float maxHealth;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        maxHealth = health;
        healthBar.SetMaxHealth(maxHealth);
    }

    public float hitPlayer(float damage = 1)
    {
        health -= damage;
        if (health <= 0) Die();
        healthBar.SetHealth(Mathf.Clamp(health, 0, maxHealth));
        Debug.Log("Player was hurt");
        return health;
    }

    void Die()
    {
        if(PlayerPrefs.GetInt("sound") == 1)
            audioSource.PlayOneShot(gameoverSound, 1.0f);
        Debug.Log("Game over");
    }

    public float Heal() {
        health = maxHealth;
        healthBar.GetComponent<Animator>().SetTrigger("heart_bit");
        healthBar.SetHealth(Mathf.Clamp(health, 0, maxHealth));
        Debug.Log("Player was healed");
        if(PlayerPrefs.GetInt("sound") == 1)
            audioSource.PlayOneShot(healSound, 1.0f);
        return health;
    }

}
