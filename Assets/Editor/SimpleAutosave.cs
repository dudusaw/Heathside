using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Heathside
{
    [InitializeOnLoad]
    public static class SimpleAutosave
    {
        private static DateTime lastSaveTime = DateTime.Now;
        private static TimeSpan updateInterval = new TimeSpan(0, 5, 0);

        static SimpleAutosave()
        {
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (EditorApplication.isPlaying) return;

            if ((DateTime.Now - lastSaveTime) >= updateInterval)
            {
                Save();
            }
        }

        private static void Save()
        {
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            lastSaveTime = DateTime.Now;
            Debug.Log("scenes saved");
        }
    }
}