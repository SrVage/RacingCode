using System.Collections;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class LoadingCanvas:MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _curtain;
        public static LoadingCanvas Instance;
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            DontDestroyOnLoad(this);
            Show();
        }

        private void Show()
        {
            _slider.value = 0;
            if (!_curtain.activeSelf)
            {
                _curtain.SetActive(true);
            }
            LoadGameScene();
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(1);
            _slider.value = 0.2f;
        }

        public void LoadLevel()
        {
            _slider.value += 0.2f;
        }

        public void Hide()
        {
            StartCoroutine(nameof(HideCurtain));
        }

        private IEnumerator HideCurtain()
        {
            while (_slider.value<1)
            {
                _slider.value += 0.1f;
                yield return null;
            }
            _curtain.SetActive(false);
        }

        public void Destroy()
        {
            Instance = null;
            GameObject.Destroy(transform.root.gameObject);
        }
    }
}