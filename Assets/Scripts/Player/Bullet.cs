using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("Used for define is gravity enebled to bullet")]
    private bool isActive = true;

    [Tooltip("Time to destroy the bullet after shoot")]
    public float destroyDelay = 3f;
    
    [Tooltip("Time to restore ConfigurableJoint of the enemy")]
    public float restoreDelay = 10f;

    private void Start()
    {
        StartCoroutine(DestroyBullet(destroyDelay));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;

        isActive = false;

        GetComponent<Rigidbody>().useGravity = true;

        if(GameHelpers.FindParentWithTag(collision.gameObject, "Enemy"))
        {
            //Debug.Log(collision.gameObject.name);

            ConfigurableJoint enemyCJ = collision.gameObject.GetComponent<ConfigurableJoint>();
            PhysicalBodyPart enemyPBP = collision.gameObject.GetComponent<PhysicalBodyPart>();

            Destroy(enemyPBP);
            Destroy(enemyCJ);
        }

    }



    IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
