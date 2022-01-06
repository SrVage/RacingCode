using UnityEngine;

namespace Code.LevelsLoader
{
    public class ChangeLevelService
    {
        private const string LevelNumber = "LevelNumber";
        public int CurrentLevel => _currentLevel;
        private int _currentLevel;
        
        public ChangeLevelService()
        {
            GetLevelNumber();
        }

        public void ChangeLevel()
        {
            _currentLevel++;
            Save();
        }

        private void Save()
        {
            PlayerPrefs.SetInt(LevelNumber, _currentLevel);
        }

        private void GetLevelNumber()
        {
           _currentLevel = PlayerPrefs.GetInt(LevelNumber, 0);
        }
    }
}