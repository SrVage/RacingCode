using UnityEngine;

namespace Code.LevelsLoader
{
    public static class SaveSpawnPointService
    {
        private const string SpawnPoint = nameof(SpawnPoint);
        
        public static void Save(int pointNumber)
        {
            PlayerPrefs.SetInt(SpawnPoint, pointNumber);
        }

        public static int Get()
        {
            return PlayerPrefs.GetInt(SpawnPoint, 0);
        }
    }
}