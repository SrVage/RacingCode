using Code.Tools;
using UnityEngine;

namespace Code.GameConfigDescription
{
    [CreateAssetMenu(order = 1, fileName = "CarCfg", menuName = "Config/Car")]
    public class CarCfg:ScriptableObject
    {
        [Tooltip("Префаб машины")] public GameObject CarPrefab;
        [Header("Настройки массы и центра тяжести")] [Tooltip("Масса машины")] public float Mass;
        [InjectConfig][Tooltip("Центра тяжести выше/ниже")][Range(0, 1.5f)] public float CenterOfMassTop;
        [InjectConfig][Tooltip("Центра тяжести вперед/назад")][Range(-1.5f, 1.5f)] public float CenterOfMassForward;

        [Header("Движение вперед")]
        [Header("Настройки скорости/ускорения")]
        [InjectConfig][Tooltip("Максимальная скорость машины")] public float MaximumSpeed;
        [InjectConfig][Tooltip("Ускорение на маленькой скорости")] public float ZeroSpeedAccelerate;
        [InjectConfig][Tooltip("Ускорение на большой скорости")] public float MaxSpeedAccelerate;
        [Header("Движение назад")]
        [InjectConfig][Tooltip("Максимальная скорость машины")] public float MaximumSpeedBack;
        [InjectConfig][Tooltip("Ускорение на маленькой скорости")] public float ZeroSpeedAccelerateBack;
        [InjectConfig][Tooltip("Ускорение на большой скорости")] public float MaxSpeedAccelerateBack;
        [Header("Настройки тормозов")] 
        [InjectConfig][Tooltip("Тормозная сила на маленькой скорости")] public float MinSpeedBrakeForce;
        [InjectConfig][Tooltip("Тормозная сила на большой скорости")] public float MaxSpeedBrakeForce;
        [Header("Настройка физики кузова")]
        [InjectConfig][Tooltip("Прыгучесть кузова")] public float Jumping;
        [InjectConfig][Tooltip("Скольжение кузова")] public float Friction;
        [Header("Антикрыло")] 
        [InjectConfig][Tooltip("Прижимная сила на высокой скорости")] public float MaxSpeedAntiWingForce;
        [InjectConfig][Tooltip("Прижимная сила на низкой скорости")] public float MinSpeedAntiWingForce;
        [Header("Другие настройки")]
        [InjectConfig][Tooltip("Распределение крутящего момента между осями. 0 - заднеприводный автомобиль, 1 - переднеприводный автомобиль")] [Range(0, 1)] public float AxisRatio;
        [InjectConfig][Tooltip("Множитель для дополнительной силы на кузов вдоль машины")] public float ForwardForceMultiply;
        [Header("Машина застряла")] 
        [InjectConfig] [Tooltip("Боковая сила на застрявшую машину")] public float SideStuckForce;
        [InjectConfig] [Tooltip("Продольная сила на застрявшую машину")] public float ForwardStuckForce;
        [Header("Машина летит")] 
        [InjectConfig] [Tooltip("Боковая сила на застрявшую машину")] public float SideFlyingForce;
        [InjectConfig] [Tooltip("Продольная сила на застрявшую машину")] public float ForwardFlyingForce;
        [InjectConfig] [Tooltip("Продольная сила на застрявшую машину")] public float BackwardFlyingForce;
    }
}