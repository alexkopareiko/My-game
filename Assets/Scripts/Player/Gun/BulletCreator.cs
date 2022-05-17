using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletCreator : MonoBehaviour
{
    [Tooltip("Bullet prefab")]
    public GameObject BulletPrefab;

    [Tooltip("Speed of bullet")]
    public float BulletVelocity = 30f;

    [Tooltip("Where the gun needs to be aimed")]
    public Transform Aim;

    [Tooltip("Sound of shooting")]
    public AudioClip shootSound;

    [Tooltip("Audio Source for Gun")]
    private AudioSource audioSource;

    private void Start()
    {
        transform.LookAt(Aim);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        ray.origin = transform.position;
        ray.direction = transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
            audioSource.PlayOneShot(shootSound, 1.0f);
        }

    }
}
