using Code.Components;
using Code.UI.Components;
using Leopotam.Ecs;

namespace Code.UI.Systems
{
    public class ChangeKeysSystem:IEcsRunSystem
    {
        private readonly EcsFilter<Keys> _keys;
        private readonly EcsFilter<KeysText> _text;
        public void Run()
        {
            foreach (var idx in _keys)
            {
                ref var currentKeys = ref _keys.Get1(idx).Key;
                foreach (var text in _text)
                {
                    ref var textKeys = ref _text.Get1(text).Text;
                    textKeys.text =  $"Keys: {currentKeys}";
                }
            }
        }
    }
}