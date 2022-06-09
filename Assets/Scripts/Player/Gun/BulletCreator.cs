using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Tooltip("Sound of recharge")]
    public AudioClip rechargeSound;
   
    [Tooltip("Sound of empty shooting")]
    public AudioClip emptyShootSound;

    public int maxBulletAmount;

    
    [Tooltip("TextMPro for bullet amount")]
    [SerializeField] private TMP_Text textBulletAmount;
    
    [Tooltip("Bullet Amount")] 
    [SerializeField] private int bulletAmount = 10;


    [Tooltip("Audio Source for Gun")]
    private AudioSource audioSource;

    [Tooltip("ShootRecoil (otdacha ot udara)")]
    private float shootRecoil = 1000f;

    [Tooltip("Time interval between shooting")]
    private float shootInterval = 0.2f;
    private float timeElapse = 0f;


    GameManager gameManager;
    FPSInput fpsInput;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;
        fpsInput = gameManager.player.GetComponent<FPSInput>();
        textBulletAmount.text = bulletAmount.ToString();
        maxBulletAmount = bulletAmount;
    }

    private void Update()
    {
        FixGunBlockInTheAir();

        transform.LookAt(Aim);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        ray.origin = transform.position;
        ray.direction = transform.forward;

        if(!GameManager.gameIsPaused) {
            Shoot();
        }
    }

    void Shoot() {
        timeElapse += Time.deltaTime;
        if (Input.GetMouseButton(0) && timeElapse > shootInterval)
        {
            timeElapse = 0;
            if(bulletAmount > 0)
            {
                GameObject newBullet = Instantiate(BulletPrefab, SourceOfBullets.position, SourceOfBullets.rotation);
                newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
                if(PlayerPrefs.GetInt("sound") == 1)
                    audioSource.PlayOneShot(shootSound, 1.0f);
                gunBlockRb.AddRelativeForce(0f, 0f, -shootRecoil, ForceMode.Force);
                bulletAmount--;
                textBulletAmount.text = bulletAmount.ToString();
            } else
            {
                if(PlayerPrefs.GetInt("sound") == 1)
                    audioSource.PlayOneShot(emptyShootSound, 1.0f);
            }

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

    public void GunRecharge() {
        bulletAmount += maxBulletAmount;
        textBulletAmount.text = bulletAmount.ToString();
        if(PlayerPrefs.GetInt("sound") == 1)
            audioSource.PlayOneShot(rechargeSound, 1.0f);
        textBulletAmount.GetComponentInParent<Animator>().SetTrigger("patron_bit");
        GetComponent<Animator>().SetTrigger("gun_recharge");
    }
}
