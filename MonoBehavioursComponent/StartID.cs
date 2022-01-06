using System;
using Code.Tools;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace Code.MonoBehavioursComponent
{
    public class StartID : MonoBehaviour
    {
        [InjectConfig] public int ID=0;
        public bool IsActive = true;

        [ExecuteInEditMode]
        public void SetID(int id)
        {
            Debug.Log(id);
            ID = id;
        }
    }
}