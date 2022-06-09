using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseLevel : MonoBehaviour
{
    [Tooltip("Text for level choosing")]
    public TMP_InputField levelChoose;

    [Tooltip("Slider for level choosing")]
    public Slider sliderLevel;

    void Start()
    {
        sliderLevel.maxValue = PlayerPrefs.GetInt("max_score");
        sliderLevel.value = PlayerPrefs.GetInt("max_score");
        levelChoose.text = PlayerPrefs.GetInt("max_score").ToString();
    }

    public void OnValueChanged() {
        int level = Mathf.Clamp(System.Int32.Parse(levelChoose.text), 1, PlayerPrefs.GetInt("max_score")) ;
        
    }

    public void OnDeselect() {
        levelChoose.text = PlayerPrefs.GetInt("level_to_load").ToString();
    }

    public void OnSliderChanged(Slider slider) {
        levelChoose.text = slider.value.ToString();
        PlayerPrefs.SetInt("level_to_load", (int)slider.value);
    }
}
