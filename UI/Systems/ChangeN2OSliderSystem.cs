using Code.Components.Boosters;
using Code.GameConfigDescription;
using Code.UI.Components;
using Leopotam.Ecs;

namespace Code.UI.Systems
{
    public class ChangeN2OSliderSystem:IEcsRunSystem
    {
        private readonly EcsFilter<N2OUIElement> _slider;
        private readonly EcsFilter<N2OEffect> _effect;
        private readonly N2OCfg _n2OCfg;
        
        public void Run()
        {
            if (_effect.IsEmpty())
            {
                foreach (var slider in _slider)
                {
                    ref var button = ref _slider.Get1(slider).Button;
                    if (button.interactable == true)
                    {
                        button.interactable = false;
                        button.onClick.RemoveAllListeners();
                        ref var n20Slider = ref _slider.Get1(slider).Slider;
                        n20Slider.value = 0;
                    }
                }
                return;
            }
            foreach (var effect in _effect)
            {
                ref var value = ref _effect.Get1(effect).Time;
                foreach (var slider in _slider)
                {
                    ref var n20Slider = ref _slider.Get1(slider).Slider;
                    n20Slider.value = value / _n2OCfg.Duration;
                    ref var button = ref _slider.Get1(slider).Button;
                    if (button.interactable == false)
                    {
                        button.interactable = true;
                        button.onClick.AddListener(()=>ButtonClick());
                    }
                }
            }
        }

        private void ButtonClick()
        {
            foreach (var effect in _effect)
            {
                ref var value = ref _effect.Get1(effect).Time;
                value = 0;
            }
        }
    }
}