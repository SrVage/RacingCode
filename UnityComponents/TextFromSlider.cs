using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UnityComponents
{
    public class TextFromSlider:MonoBehaviour
    {
        protected TextMeshProUGUI _text;
        protected virtual void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            
            var slider = transform.parent.GetComponent<Slider>();
            _text.SetText(slider.value.ToString());
            slider.onValueChanged.AddListener(value=>_text.SetText(value.ToString()));
        }
    }
}