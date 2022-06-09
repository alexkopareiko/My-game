using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDiamondsManager : MonoBehaviour
{
    public GameObject diamondPrefab;

    [Tooltip("Period for instantiating diamonds")]
    public float period = 15f;
    private float nextActionTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");
        bool isDiamondNear = false;
        foreach(GameObject diamond in diamonds) {
            float distance = Vector3.Distance(diamond.transform.position, transform.position);
            if(distance == 0) {
                isDiamondNear = true;
            }
        }
        if (Time.time > nextActionTime && !isDiamondNear) {
            nextActionTime += period;
            Instantiate(diamondPrefab, transform.position, diamondPrefab.transform.rotation);
        }
    }
}
