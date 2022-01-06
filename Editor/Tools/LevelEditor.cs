using System;
using System.Linq;
using System.Reflection;
using Code.MonoBehavioursComponent;
using Code.Tools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Editor.Tools
{
    public class LevelEditor:EditorWindow
    {
        private Object _startObject;
        private Object _endObject;
        private Object _idObject;
        private GameObject start;
        private GameObject end;
        private GameObject id;
        [MenuItem("Tools/Level Editor")]
        public static void Editor()
        {
            GetWindow<LevelEditor>("Level Editor");
        }

        private void OnGUI()
        {
            _startObject = EditorGUILayout.ObjectField("Объект, к которому стыкуем", _startObject, typeof(GameObject), true) as GameObject;
            _endObject = EditorGUILayout.ObjectField("Стыкуемый объект", _endObject, typeof(GameObject), true) as GameObject;
            start = _startObject as GameObject;
            end = _endObject as GameObject;
            if (GUILayout.Button("Dock"))
            {
                if (start != null && end != null)
                    Dock(start.transform, end.transform);
            }
            _idObject = EditorGUILayout.ObjectField("Уровень для проставления ID", _idObject, typeof(GameObject), true) as GameObject;
            id = _idObject as GameObject;
            if (GUILayout.Button("Set ID"))
            {
                if (id != null)
                {
                    int num = 0;
                    var ids = id.GetComponentsInChildren<StartID>();
                    for (int i = 0; i < ids.Length; i++)
                    {
                        var type = ids[i].GetType();
                        var allFields = type
                            .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                            .FirstOrDefault(field => CustomAttributeExtensions.GetCustomAttribute((MemberInfo)field, typeof(InjectConfigAttribute)) != null);
                        allFields.SetValue(ids[i], num++);
                    }
                } 
            }
        }
        
        [CustomEditor(typeof(StartID))]
        

        private void Dock(Transform start, Transform end)
        {
            end.rotation = start.rotation;
            var point1 = start.GetChild(1).transform.position;
            var point2 = end.GetChild(0).transform.position;
            var vector = point1 - point2;
            end.position += vector;
        }
    }
}