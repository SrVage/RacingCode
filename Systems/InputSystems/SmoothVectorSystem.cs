using Code.Components;
using Code.Extensions;
using Code.GameConfigDescription;
using Leopotam.Ecs;
using UnityEngine;

namespace Code.Systems.InputSystems
{
    public class SmoothVectorSystem:IEcsRunSystem
    {
        private readonly EcsFilter<InputVector> _input;
        private readonly EcsFilter<CollectionVectorForSmooth> _smoothVector;
        private readonly EcsWorld _world;
        private readonly ControlCfg _controlCfg;
        public void Run()
        {
            if (_input.IsEmpty())
            {
                foreach (var smooth in _smoothVector)
                {
                    ref var smoothCollection = ref _smoothVector.GetEntity(smooth);
                    smoothCollection.Destroy();
                }
            }
            if (_smoothVector.IsEmpty())
            {
                _world.NewEntity().Get<CollectionVectorForSmooth>().Smooth = new Vector2[(int)_controlCfg.SmoothingDegree];
            }
            foreach (var input in _input)
            {
                ref var endVector = ref _input.Get1(input).EndPoint;
                ref var startVector = ref _input.Get1(input).CurrentPoint;
                Vector2 vector = endVector - startVector;
                foreach (var smooth in _smoothVector)
                {
                    ref var smoothCollection = ref _smoothVector.Get1(smooth).Smooth;
                    smoothCollection.AddVector(vector);
                    _world.NewEntity().Get<SmoothVector>().Vector = smoothCollection.SmoothVector();
                }
            }
        }
    }
}