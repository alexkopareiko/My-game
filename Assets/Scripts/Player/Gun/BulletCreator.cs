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

    GameManager gameManager;
    FPSInput fpsInput;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
        fpsInput = gameManager.player.GetComponent<FPSInput>();
    }

    private void Update()
    {

        FixGunBlockInTheAir();

        transform.LookAt(Aim);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        ray.origin = transform.position;
        ray.direction = transform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(BulletPrefab, SourceOfBullets.position, SourceOfBullets.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
            audioSource.PlayOneShot(shootSound, 1.0f);

            gunBlockRb.AddRelativeForce(0f, 0f, -shotRecoil, ForceMode.Force);
        }


    }

    // to prevent gunBlock jerking in the air while pressing move buttons
    private void FixGunBlockInTheAir()
    {
        bool isGrounded = fpsInput.IsGrounded;
        if (!isGrounded)
        {
            gunBlockRb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            gunBlockRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }
}
