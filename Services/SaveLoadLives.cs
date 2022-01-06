using UnityEngine;

namespace Code.Services
{
    public class SaveLoadLives
    {
        private const string Lives = "Lives";

        public SaveLoadLives(int currentLives)
        {
            if (PlayerPrefs.HasKey(Lives))
                return;
            PlayerPrefs.SetInt(Lives, currentLives);
        }

        public void Clear() => 
            PlayerPrefs.DeleteKey(Lives);

        public int Get() => 
            PlayerPrefs.GetInt(Lives);

        public void Save(int currentLives) => 
            PlayerPrefs.SetInt(Lives, currentLives);
    }
}