using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{

    [Tooltip("1 - heal, 2 - gunRecharge")]
    private int whatToDo = 1;

    

    // Start is called before the first frame update
    void Start()
    {
        whatToDo = Random.Range(1, 2+1);
    }

    private void OnTriggerEnter(Collider other) {
        Player player = other.gameObject.GetComponent<Player>();
        if(player) {
            DoSomething(whatToDo, other.gameObject);
        }
    }

    void DoSomething(int action, GameObject player) {
        if(action == 1) {
            player.GetComponent<Player>().Heal();
        }
        else if(action == 2) {
            player.GetComponentInChildren<BulletCreator>().GunRecharge();
        }
        Destroy(gameObject);
    }
}
