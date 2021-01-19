using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Saving
{
    public interface ISaveable
    {
        string SaveID { get; }
        /// <summary>
        /// All saved objects must have a [System.Serializable] attribute or be serializable by default like primitive types.
        /// </summary>
        object SavedData { get; }
        void LoadFromData(object data);
    }

    public static class SavingService
    {
        private const string ACTIVE_SCENE_KEY = "activeScene";
        private const string SCENES_KEY = "scenes";
        private static UnityEngine.Events.UnityAction<Scene, LoadSceneMode> LoadObjectsAfterSceneLoad;

        public static void SaveGame(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            List<ISaveable> saveables = GetSaveables();
            Dictionary<string, object> savedObjects = new Dictionary<string, object>();

            if (saveables.Count > 0)
            {
                foreach (var item in saveables)
                {
                    savedObjects[item.SaveID] = item.SavedData;
                }
            }
            else
            {
                Debug.LogWarningFormat("The scene did not include any saveable objects.");
            }

            var sceneCount = SceneManager.sceneCount;
            List<string> sceneNames = new List<string>();
            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames.Add(SceneManager.GetSceneAt(i).name);
            }
            savedObjects[SCENES_KEY] = sceneNames;
            savedObjects[ACTIVE_SCENE_KEY] = SceneManager.GetActiveScene().name;

            Stream fileStream = GetStream(fileName, FileMode.Create, FileAccess.Write);
            formatter.Serialize(fileStream, savedObjects);
            fileStream.Close();
            Debug.Log("Save success");
            System.GC.Collect();
        }

        public static bool LoadGame(string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream fileStream = GetStream(fileName, FileMode.Open, FileAccess.Read);
            if (fileStream == null)
            {
                return false;
            }
            object graph = formatter.Deserialize(fileStream);
            fileStream.Close();

            if (graph is Dictionary<string, object> savedObjects)
            {
                if (!savedObjects.ContainsKey(SCENES_KEY))
                {
                    Debug.LogWarningFormat("Data at {0} does not contain any scenes; not " + "loading any!", fileName);
                    return false;
                }
                List<string> scenes = (List<string>)savedObjects[SCENES_KEY];
                int sceneCount = scenes.Count;
                if (sceneCount == 0)
                {
                    Debug.LogWarningFormat("Data at {0} doesn't specify any scenes to load.", fileName);
                    return false;
                }

                for (int i = 0; i < sceneCount; i++)
                {
                    string scene = scenes[i];
                    if (i == 0)
                    {
                        SceneManager.LoadScene(scene, LoadSceneMode.Single);
                    }
                    else
                    {
                        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
                    }
                }

                if (savedObjects.ContainsKey(ACTIVE_SCENE_KEY))
                {
                    var activeSceneName = (string)savedObjects[ACTIVE_SCENE_KEY];
                    var activeScene = SceneManager.GetSceneByName(activeSceneName);
                    if (activeScene.IsValid() == false)
                    {
                        Debug.LogErrorFormat(
                        "Data at {0} specifies an active scene that " +
                        "doesn't exist. Stopping loading here.",
                        fileName);
                        return false;
                    }
                    SceneManager.SetActiveScene(activeScene);
                }
                else
                {
                    Debug.LogWarningFormat("Data at {0} does not specify an " +
                    "active scene.", fileName);
                }

                LoadObjectsAfterSceneLoad = (scene, loadSceneMode) =>
                {
                    List<ISaveable> saveables = GetSaveables();
                    foreach (var item in saveables)
                    {
                        string saveId = item.SaveID;
                        if (savedObjects.ContainsKey(saveId))
                        {
                            item.LoadFromData(savedObjects[saveId]);
                        }
                    }
                    SceneManager.sceneLoaded -= LoadObjectsAfterSceneLoad;
                    LoadObjectsAfterSceneLoad = null;
                    System.GC.Collect();
                };

                SceneManager.sceneLoaded += LoadObjectsAfterSceneLoad;
            }
            else
            {
                Debug.LogWarningFormat($"Could not deserialize file {fileName}");
                return false;
            }

            Debug.Log("Load success");
            return true;
        }

        private static Stream GetStream(string fileName, FileMode mode, FileAccess access)
        {
            var outputPath = Path.Combine(Application.persistentDataPath, fileName);
            if (mode == FileMode.Open && !File.Exists(outputPath))
            {
                Debug.LogErrorFormat("No file exists at {0}", outputPath);
                return null;
            }
            return new FileStream(outputPath, mode, access);
        }

        private static List<ISaveable> GetSaveables()
        {
            MonoBehaviour[] saveableBehaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            List<ISaveable> saveables = new List<ISaveable>();
            foreach (var item in saveableBehaviours)
            {
                if (item is ISaveable savable)
                {
                    saveables.Add(savable);
                }
            }
            return saveables;
        }
    }
}

