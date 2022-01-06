using Code.MonoBehavioursComponent;
using Code.StatesSwitcher.Events;
using Leopotam.Ecs;
using UnityEngine.EventSystems;

namespace Code.UI
{
    public class TTSScreen:UIEntity, IPointerDownHandler
    {
        public override void Initial(EcsWorld world)
        {
            base.Initial(world);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _world.NewEntity().Get<TapToStart>();
        }
    }
}