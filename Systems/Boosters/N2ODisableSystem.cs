using Code.Components;
using Code.Components.Boosters;
using Code.GameConfigDescription;
using DG.Tweening;
using Leopotam.Ecs;

namespace Code.Systems.Boosters
{
    public class N2ODisableSystem:IEcsRunSystem
    {
        private readonly EcsFilter<N2OEntity, GameObjectRef, Destroy> _n2o;
        private readonly N2OCfg _n2OCfg;
        public void Run()
        {
            foreach (var n2o in _n2o)
            {
                ref var transform = ref _n2o.Get2(n2o).Transform;
                var collider = _n2o.Get1(n2o).Collider;
                ref var entity = ref _n2o.GetEntity(n2o);
                entity.Del<Destroy>();
                collider.enabled = false;
                DOTween.Sequence().Append(transform.DOScale(0.01f, 0.5f)).AppendInterval(_n2OCfg.RespawnTime)
                    .Append(transform.DOScale(1, 0.5f)).OnComplete(() =>
                    {
                        collider.enabled = true;
                    });
            }
        }
    }
}