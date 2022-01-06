using UnityEngine;

namespace Code.GameConfigDescription
{
    [CreateAssetMenu(order = 7, fileName = "N2OCfg", menuName = "Config/Boosters/N2O")]
    public class N2OCfg:ScriptableObject
    {
        public float Duration;
        public float MaxSpeedMultiplier;
        public float ForwardAccelerationMultiplier;
        public float RespawnTime;
    }
}