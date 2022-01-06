using Code.MonoBehavioursComponent;
using Code.StatesSwitcher.Events;
using Code.UI.Components;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class GameplayScreen:UIEntity
    {
        [SerializeField] private Button _reset;
        [SerializeField] private Button _clear;
        [SerializeField] private Button _config;
        [SerializeField] private TextMeshProUGUI _speed;
        [SerializeField] private TextMeshProUGUI _wheelSpeed;
        [SerializeField] private TextMeshProUGUI _coins;
        [SerializeField] private TextMeshProUGUI _lives;
        [SerializeField] private TextMeshProUGUI _keys;
        [SerializeField] private Slider _n2oSlider;
        [SerializeField] private Button _n2oButton;
        public override void Initial(EcsWorld world)
        {
            base.Initial(world);
            ref var speed = ref world.NewEntity().Get<SpeedText>();
                speed.Speed = _speed;
                speed.WheelSpeed = _wheelSpeed;
            world.NewEntity().Get<CoinText>().Text = _coins;
            world.NewEntity().Get<LivesText>().Text = _lives;
            world.NewEntity().Get<KeysText>().Text = _keys;
            ref var uiElement = ref world.NewEntity().Get<N2OUIElement>();
            uiElement.Slider = _n2oSlider;
            uiElement.Button = _n2oButton;
            _reset.onClick.AddListener(ResetGame);
            _clear.onClick.AddListener(Clear);
            _config.onClick.AddListener(OpenConfig);
        }

        private void ResetGame()
        {
            _reset.onClick.RemoveAllListeners();
            _clear.onClick.RemoveAllListeners();
            _config.onClick.RemoveAllListeners();
            _world.NewEntity().Get<ResetGame>();
        }

        private void Clear()
        {
            PlayerPrefs.DeleteAll();
            _reset.onClick.RemoveAllListeners();
            _clear.onClick.RemoveAllListeners();
            _config.onClick.RemoveAllListeners();
            LoadingCanvas.Instance.Destroy();
            SceneManager.LoadScene(0);
        }

        private void OpenConfig()
        {
            _world.NewEntity().Get<StartConfig>();
        }
    }
}