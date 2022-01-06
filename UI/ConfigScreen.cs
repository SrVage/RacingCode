using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Code.GameConfigDescription;
using Code.MonoBehavioursComponent;
using Code.StatesSwitcher.Events;
using Code.Tools;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class ConfigScreen:UIEntity
    {
        private const int Space = 150;

        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _controlButton;
        [SerializeField] private Button _carButton;
        [SerializeField] private Button _wheelsButton;
        [SerializeField] private Button _cameraButton;
        [SerializeField] private Button _levelButton;
        [SerializeField] private SliderMonoBehaviour _sliderPrefab;
        [Header("Panels")]
        [SerializeField] private GameObject _controlPanel;
        [SerializeField] private GameObject _carPanel;
        [SerializeField] private GameObject _wheelsPanel;
        //[SerializeField] private GameObject _cameraPanel;
        //[SerializeField] private GameObject _levelPanel;
        [Header("ControlConfigs")] 
        [SerializeField] private ControlCfg _controlCfg;
        [SerializeField] private Slider _controlScroll;
        [Header("CarConfig")]
        [SerializeField] private CarCfg _carCfg;
        [SerializeField] private Slider _carScroll;

        [Header("WheelsConfig")] 
        [SerializeField] private WheelCfg _wheelCfg;
        [SerializeField] private Slider _wheelScroll;

        private List<Slider> _sliders;

        public override void Initial(EcsWorld world)
        {
            base.Initial(world);
            _sliders = new List<Slider>();
            int count = 0;
            var type = _carCfg.GetType();
            CreateSlider(type, _carPanel.transform, _carCfg, out count);

            _carPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.up * _carScroll.value;
            _carScroll.onValueChanged.AddListener(a=>_carPanel.GetComponent<RectTransform>().anchoredPosition=Vector2.up*a);
            _carScroll.maxValue = Space * count;
            _sliders.Add(_carScroll);
            
            type = _controlCfg.GetType();
            CreateSlider(type, _controlPanel.transform, _controlCfg, out count);
            
            _controlPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.up * _controlScroll.value;
            _controlScroll.onValueChanged.AddListener(a=>_controlPanel.GetComponent<RectTransform>().anchoredPosition=Vector2.up*a);
            _controlScroll.maxValue = Space * count;
            _sliders.Add(_controlScroll);
            
            type = _wheelCfg.GetType();
            CreateSlider(type, _wheelsPanel.transform, _wheelCfg, out count);
            
            _wheelsPanel.GetComponent<RectTransform>().anchoredPosition = Vector2.up * _wheelScroll.value;
            _wheelScroll.onValueChanged.AddListener(a=>_wheelsPanel.GetComponent<RectTransform>().anchoredPosition=Vector2.up*a);
            _wheelScroll.maxValue = Space * count;
            _sliders.Add(_wheelScroll);
            
            _closeButton.onClick.AddListener(Accept);
            _controlButton.onClick.AddListener(ShowControl);
            _carButton.onClick.AddListener(ShowCar);
            _wheelsButton.onClick.AddListener(ShowWheels);
            //_cameraButton.onClick.AddListener(ShowCamera);
            //_levelButton.onClick.AddListener(ShowLevel);
            DiselectAll();
        }

        private void CreateSlider(Type type, Transform parent, object value, out int count)
        {
            var allFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(field => field.GetCustomAttribute(typeof(InjectConfigAttribute)) != null).ToArray();
            count = allFields.Length;
            foreach (var field in allFields)
            {
                var obj = Instantiate(_sliderPrefab, Vector3.zero, Quaternion.identity, parent);
                obj.ChangeName(field.Name);
                var slider = obj.GetSlider();
                _sliders.Add(slider);
                obj.ChangeRightBorderValue(field.GetValue(value).ToString());
                obj.ChangeLeftBorderValue(field.GetValue(value).ToString());
                slider.value = (float)field.GetValue(value);
                slider.onValueChanged.AddListener(a => field.SetValue(value, (float)Math.Round(a, 2)));
            }
        }


        private void Accept()
        {
            Debug.Log("click");
            _closeButton.onClick.RemoveAllListeners();
            _controlButton.onClick.RemoveAllListeners();
            _carButton.onClick.RemoveAllListeners();
            _wheelsButton.onClick.RemoveAllListeners();
            _cameraButton.onClick.RemoveAllListeners();
            _levelButton.onClick.RemoveAllListeners();
            foreach (var slider in _sliders)
            {
                slider.onValueChanged.RemoveAllListeners();
            }
            LoadingCanvas.Instance.Destroy();
            SceneManager.LoadScene(0);
            //_world.NewEntity().Get<ResetGame>();
        }

        #region Button
        private void ShowControl()
        {
            DiselectAll();
            _controlPanel.transform.parent.gameObject.SetActive(true);
            _controlButton.interactable = false;
        }
        
        private void ShowCar()
        {
            DiselectAll();
            _carPanel.transform.parent.gameObject.SetActive(true);
            _carButton.interactable = false;
        }  
        
        private void ShowWheels()
        {
            DiselectAll();
            _wheelsPanel.transform.parent.gameObject.SetActive(true);
            _wheelsButton.interactable = false;
        }        
        
        private void ShowCamera()
        {
            DiselectAll();
            //_cameraPanel.SetActive(true);
            _cameraButton.interactable = false;
        }   
        
        private void ShowLevel()
        {
            DiselectAll();
            //_levelPanel.SetActive(true);
            _levelButton.interactable = false;
        }

        private void DiselectAll()
        {
            _controlPanel.transform.parent.gameObject.SetActive(false);
            _carPanel.transform.parent.gameObject.SetActive(false);
            _wheelsPanel.transform.parent.gameObject.SetActive(false);
            //_cameraPanel.SetActive(false);
            //_levelPanel.SetActive(false);
            _controlButton.interactable = true;
            _carButton.interactable = true;
            _wheelsButton.interactable = true;
            //_cameraButton.interactable = true;
            //_levelButton.interactable = true;
        }
        #endregion

        
        private void OnDestroy()
        {
            /*_controlButton.onClick.RemoveAllListeners();
            _carButton.onClick.RemoveAllListeners();
            _wheelsButton.onClick.RemoveAllListeners();
            _cameraButton.onClick.RemoveAllListeners();
            _levelButton.onClick.RemoveAllListeners();
            foreach (var slider in _sliders)
            {
                slider.onValueChanged.RemoveAllListeners();
            }*/
        }
    }
}