using Code.MonoBehavioursComponent;
using UnityEngine;

namespace Code.LevelsLoader
{
    public class SetSpawnOnLoad
    {
        public Vector3 GetSpawnPoint(GameObject level, int number)
        {
            Vector3 spawnPoint = level.transform.position+3*Vector3.forward;
            var spawnPoints = level.GetComponentsInChildren<CheckPointMonoBehaviour>();
            foreach (var spawn in spawnPoints)
            {
                if (spawn.SpawnNumber == number)
                {
                    spawnPoint = spawn.transform.position;
                    break;
                }
            }
            return spawnPoint;
        }
    }
}