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
         if(!PlayerPrefs.HasKey("maxScore"))
        {
            PlayerPrefs.SetInt("maxScore", 0);
        }
        sliderLevel.maxValue = PlayerPrefs.GetInt("maxScore");
        sliderLevel.value = PlayerPrefs.GetInt("maxScore");
        levelChoose.text = PlayerPrefs.GetInt("maxScore").ToString();
    }

    public void OnValueChanged() {
        int level = Mathf.Clamp(System.Int32.Parse(levelChoose.text), 1, PlayerPrefs.GetInt("maxScore")) ;
    }

    public void OnDeselect() {
        levelChoose.text = PlayerPrefs.GetInt("levelToLoad").ToString();
    }

    public void OnSliderChanged(Slider slider) {
        levelChoose.text = slider.value.ToString();
        PlayerPrefs.SetInt("levelToLoad", (int)slider.value);
        PlayerPrefs.Save();
    }
}
