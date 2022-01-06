using Code.MonoBehavioursComponent;
using Code.StatesSwitcher.Events;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class WinScreen:UIEntity
    {
        [SerializeField] private Button _nextLevelButton;
        public override void Initial(EcsWorld world)
        {
            base.Initial(world);
            _nextLevelButton.onClick.AddListener(NextLevel);
        }

        private void NextLevel()
        {
            _nextLevelButton.onClick.RemoveAllListeners();
            _world.NewEntity().Get<NextLevel>();
        }
    }
}