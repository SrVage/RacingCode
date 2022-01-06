using System;
using UnityEngine;

namespace Code.Services
{
    public class SaveLoadCollectable:ISaveLoadCollectable
    {
        private int[] _collectableID;
        private const string CollectableList = nameof(CollectableList);
        private string _ids;

        public void SetList(int count)
        {
            _collectableID = null;
            _collectableID = new int[count];
            for (int i = 0; i < count; i++)
            {
                _collectableID[i] = 2;
            }
        }

        public int[] GetList()
        {
            if (_collectableID!=null)
                return _collectableID;
            Load();
            return _collectableID;
        }

        public void ChangeCollectableByID(int ID)
        {
            _collectableID[ID] = 1;
            ChangeToString();
        }

        public void DeleteList()
        {
            PlayerPrefs.DeleteKey(CollectableList);
        }

        public bool IsSaved()
        {
            return PlayerPrefs.HasKey(CollectableList);

        }

        private void Save()
        {
            PlayerPrefs.SetString(CollectableList, _ids);
        }

        private void Load()
        {
            _ids = PlayerPrefs.GetString(CollectableList);
            var charArray = _ids.ToCharArray();
            SetList(charArray.Length);
            for (int i = 0; i < charArray.Length; i++)
            {
                _collectableID[i] =charArray[i]-'0';
            }
        }

        private void ChangeToString()
        {
            _ids = String.Empty;
            for (int i = 0; i < _collectableID.Length; i++)
            {
                _ids += _collectableID[i].ToString();
            }
            Save();
        }
    }
}