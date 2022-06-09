using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{

    public Material healthMaterial;
    public Material patronsMaterial;

    [Tooltip("Time to destroy the diamond self")]
    public float destroyDelay = 30f;

    [Tooltip("1 - heal, 2 - gunRecharge")]
    private int whatToDo = 1;

    private const int HEAL = 1;
    private const int GUN_RECHARGE = 2;

    

    // Start is called before the first frame update
    void Start()
    {
        whatToDo = Random.Range(1, 2+1);
        if(whatToDo == HEAL) {
            GetComponent<Renderer>().material = healthMaterial;
        }
        else if(whatToDo == GUN_RECHARGE) {
            GetComponent<Renderer>().material = patronsMaterial;
        }
        StartCoroutine(DestroySelf(destroyDelay));
    }

    private void OnTriggerEnter(Collider other) {
        Player player = other.gameObject.GetComponent<Player>();
        if(player) {
            DoSomething(whatToDo, other.gameObject);
        }
    }

    void DoSomething(int action, GameObject player) {
        if(action == HEAL) {
            player.GetComponent<Player>().Heal();
        }
        else if(action == GUN_RECHARGE) {
            player.GetComponentInChildren<BulletCreator>().GunRecharge();
        }
        Destroy(gameObject);
    }

    IEnumerator DestroySelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
