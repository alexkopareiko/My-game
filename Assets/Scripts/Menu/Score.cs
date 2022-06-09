using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("max_score").ToString();
    }
}
