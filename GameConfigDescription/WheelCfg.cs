using Code.Tools;
using UnityEngine;

namespace Code.GameConfigDescription
{
    [CreateAssetMenu(order = 5, fileName = "WheelCfg", menuName = "Config/Wheels")]
    public class WheelCfg:ScriptableObject
    {
        [InjectConfig][Tooltip("Масса колеса")] public float Mass;
        
        [Header("Настройки подвески для передней оси")]
        [InjectConfig][Tooltip("Высота подвески")] public float ForwardClearance;
        [InjectConfig][Tooltip("Сила пружин")] public float ForwardSpring;
        [InjectConfig][Tooltip("Амортизаторы")] public float ForwardDamping;
        
        [Header("Настройки подвески для задней оси")]
        [InjectConfig][Tooltip("Высота подвески")] public float BackClearance;
        [InjectConfig][Tooltip("Сила пружин")] public float BackSpring;
        [InjectConfig][Tooltip("Амортизаторы")] public float BackDamping;
        
        [Header("Движение вперед")]
        [Header("Гироскопический эффект")]
        [InjectConfig]public float LowSpeedLimit;
        [InjectConfig]public float HighSpeedLimit;
        [Header("Движение назад")]
        [InjectConfig]public float LowSpeedLimitBack;
        [InjectConfig]public float HighSpeedLimitBack;
        
        [Header("Движение вперед")]
        [InjectConfig][Tooltip("Угол поворота на минимальной скорости")] public float LowSpeedMaxAngle;
        [InjectConfig][Tooltip("Скорость поворота колес (чем больше-тем быстрей) на низкой скорости")]public float TurnSpeedOnLowSpeed;
        [InjectConfig][Tooltip("Скорость возврата колес (чем больше-тем быстрей) на низкой скорости")]public float ReturnSpeedOnLowSpeed;
        [Header("Движение назад")]
        [InjectConfig][Tooltip("Угол поворота на минимальной скорости")] public float LowSpeedMaxAngleBack;
        [InjectConfig][Tooltip("Скорость поворота колес (чем больше-тем быстрей) на низкой скорости")]public float TurnSpeedOnLowSpeedBack;
        [InjectConfig][Tooltip("Скорость возврата колес (чем больше-тем быстрей) на низкой скорости")]public float ReturnSpeedOnLowSpeedBack;
        
        [Header("Движение вперед")]
        [InjectConfig][Tooltip("Угол поворота на максимальной скорости")] public float HighSpeedMaxAngle;
        [InjectConfig][Tooltip("Скорость поворота колес (чем больше-тем быстрей) на высокой скорости")]public float TurnSpeedOnHighSpeed;
        [InjectConfig][Tooltip("Скорость возврата колес (чем больше-тем быстрей) на высокой скорости")]public float ReturnSpeedOnHighSpeed;
        [Header("Движение назад")]
        [InjectConfig][Tooltip("Угол поворота на максимальной скорости")] public float HighSpeedMaxAngleBack;
        [InjectConfig][Tooltip("Скорость поворота колес (чем больше-тем быстрей) на высокой скорости")]public float TurnSpeedOnHighSpeedBack;
        [InjectConfig][Tooltip("Скорость возврата колес (чем больше-тем быстрей) на высокой скорости")]public float ReturnSpeedOnHighSpeedBack;

        [Header("")]
        [InjectConfig][Tooltip("Накат. Чем больше, тем останавливается быстрей")] public float Roll;
        
        [Header("Передняя ось")]
        [InjectConfig][Tooltip("Боковое трение, влияет на управляемость")] public float ForwardSideFriction;
        [InjectConfig][Tooltip("Трение, влияет на разгон и торможение")] public float ForwardFriction;
        
        [Header("Задняя ось")]
        [InjectConfig][Tooltip("Боковое трение, влияет на занос")] public float BackSideFriction;
        [InjectConfig][Tooltip("Трение, влияет на разгон и торможение")] public float BackFriction;
    }
}