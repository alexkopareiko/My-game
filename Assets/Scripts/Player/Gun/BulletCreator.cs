using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletCreator : MonoBehaviour
{
    [Tooltip("Bullet prefab")]
    public GameObject BulletPrefab;
    
    [Tooltip("Gun child block Rigidbody")]
    public Rigidbody gunBlockRb;

    [Tooltip("Speed of bullet")]
    public float BulletVelocity = 30f;

    [Tooltip("Where the gun needs to be aimed")]
    public Transform Aim;
    
    [Tooltip("Source of bullets")]
    public Transform SourceOfBullets;

    [Tooltip("Sound of shooting")]
    public AudioClip shootSound;

    [Tooltip("Audio Source for Gun")]
    private AudioSource audioSource;

    [Tooltip("ShotRecoil")]
    private float shotRecoil = 1000f;

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
            GameObject newBullet = Instantiate(BulletPrefab, SourceOfBullets.position, SourceOfBullets.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
            audioSource.PlayOneShot(shootSound, 1.0f);

            gunBlockRb.AddRelativeForce(0f, 0f, -shotRecoil, ForceMode.Force);
            //gunBlockRb.AddForce(gunBlockRb.gameObject.transform.forward * 50000f, ForceMode.Impulse);
            //gunBlockRb.AddForce(transform.right * 2000f, ForceMode.Impulse);
            //gunBlockRb.AddForce(gunBlockRb.gameObject.transform.forward * 10000f, ForceMode.Impulse);
        }

    }
}
