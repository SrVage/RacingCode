using UnityEditor;
using UnityEngine;

namespace Code.Editor.Tools
{
    public static class Prefs
    {
        [MenuItem("Tools/ClearPrefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}