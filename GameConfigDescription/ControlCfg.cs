using Code.Tools;
using UnityEngine;

namespace Code.GameConfigDescription
{
    [CreateAssetMenu(order = 4, fileName = "ControlCfg", menuName = "Config/Control")]
    public class ControlCfg:ScriptableObject
    {
        [Header("Настройки чувствительностии")]
        [InjectConfig][Tooltip("Чувствительность на свайп по оси Y")] public float YSensitivity;
        [InjectConfig][Tooltip("Степень сглаживания входного вектора")]public float SmoothingDegree;
        [InjectConfig][Tooltip("Минимальный угол для поворота")]public float LimitMinAngle;
        
        [Header("Красные зоны")]
        [InjectConfig][Tooltip("Угол торможения за машиной")] [Range(0.1f, 89.9f)] public float BackAngle;
        [InjectConfig][Tooltip("Угол торможения перед машиной")] [Range(0.1f, 89.9f)] public float FrontAngle;
        [InjectConfig][Tooltip("Минимальное ускорения для поворота вне красной зоны")] public float MinAccelerate;
        
        [Header("При нулевой скорости")]
        [InjectConfig][Tooltip("Угол свайпа для разворота")] [Range(0.1f, 89.9f)] public float SideAngle;

        [Header("Ограничения свайпа")] 
        [InjectConfig][Tooltip("Минимальный размер по оси X. Если будет меньше - то будет приводится к этому значению")] public float MinVectorX;
        [InjectConfig][Tooltip("Максимальный размер по оси X. Если будет больше - то будет приводится к этому значению")]public float MaxVectorX;
        [InjectConfig][Tooltip("Минимальный размер по оси Y. Если будет меньше - то будет приводится к этому значению")] public float MinVectorY;
        [InjectConfig][Tooltip("Максимальный размер по оси Y. Если будет больше - то будет приводится к этому значению")]public float MaxVectorY;
        
    }
}