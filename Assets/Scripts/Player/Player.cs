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
        Debug.Log("Game over");
    }

}
