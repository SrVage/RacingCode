using System;
using Code.UnityComponents;
using TMPro;
using UnityEngine.UI;

namespace Code.MonoBehavioursComponent
{
    public class TextFromSliderRound:TextFromSlider
    {
        protected override void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
            var slider = transform.parent.GetComponent<Slider>();
            _text.SetText(slider.value.ToString());
            slider.onValueChanged.AddListener(value=>_text.SetText(Math.Round(value, 3).ToString()));
        }
    }
}