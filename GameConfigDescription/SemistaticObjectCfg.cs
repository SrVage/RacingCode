using UnityEngine;

namespace Code.GameConfigDescription
{
    [CreateAssetMenu(order = 6, fileName = "SemistaticCfg", menuName = "Config/Semistatic")]
    public class SemistaticObjectCfg:ScriptableObject
    {
        public float MaxTimeInTopPoint;
        public float MinTimeInTopPoint;
        public float MaxTimeInDownPoint;
        public float MinTimeInDownPoint;
        [Range(1, 5)] public float MultiplyDuration;
    }
}