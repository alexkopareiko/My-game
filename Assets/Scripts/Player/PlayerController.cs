using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    public float speed = 60.0f;
    public bool isOnGround = true;
    public float jumpForce = 600;
    public float gravityModifier;
    public Transform targetPoint;


    void Start()
    {
        //Cursor.visible = false;
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");
        if(isOnGround)
        {
            Vector3 forwardVector = transform.forward * forwardInput;
            Vector3 sideVector = transform.right * sideInput;
            playerRb.AddForce((forwardVector + sideVector) * speed, ForceMode.Acceleration);
            //playerRb.AddForce(transform.right * sideInput * speed, ForceMode.Acceleration);
        }

        if (Input.GetButtonDown("Jump") && isOnGround )
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
