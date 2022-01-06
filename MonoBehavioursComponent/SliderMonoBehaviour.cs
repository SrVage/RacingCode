using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.MonoBehavioursComponent
{
    public class SliderMonoBehaviour:MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _leftBorder;
        [SerializeField] private TMP_InputField _rightBorder;
        [SerializeField] private TextMeshProUGUI _name;
        private void Awake()
        {
            _leftBorder.onEndEdit.AddListener(ChangeLeftBorderValue);
            _rightBorder.onEndEdit.AddListener(ChangeRightBorderValue);
        }

        public Slider GetSlider()
        {
            return _slider;
        }

        public void ChangeName(string name)
        {
            _name.SetText(name);
        }

        public void ChangeLeftBorderValue(string value)
        {
            float border = 0;
            float.TryParse(value, out border);
            if (border < 0)
                border *= 2;
            else
            {
                border = border/2 - 1;
            }
            _slider.minValue = border;
        }        
        public void ChangeRightBorderValue(string value)
        {
            float border = 0;
            float.TryParse(value, out border);
            if (border >= 10)
                border *= 2;
            else
                border += 1;
            _slider.maxValue = border;
        }
    }
}