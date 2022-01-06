using UnityEngine;

namespace Code.GameConfigDescription
{
    [CreateAssetMenu(order = 5, fileName = "PlayerCfg", menuName = "Config/Player")]
    public class PlayerCfg:ScriptableObject
    {
        [SerializeField] public int MaxLives;
        [SerializeField] public int StartLives;
    }
}