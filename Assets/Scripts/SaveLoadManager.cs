using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
//using UnityEditor.SceneManagement;

public static class SaveLoadManager
{
    private static string savePath = Application.persistentDataPath + "/savegame.dat";
    public static bool savingInProgress = false;

    public static void SaveGame(string saveName)
    {
        if (savingInProgress)
        {
            Debug.Log("A save is already in progress. Please wait until it completes.");
            return;
        }

        savingInProgress = true;

        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        string fullPath = Path.Combine(folderPath, saveName + ".save");

        // Save current scene
        Scene currentScene = SceneManager.GetActiveScene();
        if (!currentScene.isDirty)
        {
            Debug.Log("Current scene has no unsaved changes. Skipping save.");
        }
        else
        {
            SaveScene(currentScene.buildIndex, fullPath, false);
        }

        // Save other game data as necessary
        // ...

        savingInProgress = false;
    }
    public static void SaveScene(int sceneIndex, string fullPath, bool overwrite)
    {
        Scene currentScene = SceneManager.GetSceneByBuildIndex(sceneIndex);

        if (!currentScene.IsValid())
        {
            Debug.LogError($"Scene at index {sceneIndex} is not valid!");
            return;
        }

        if (overwrite || !File.Exists(fullPath))
        {
            //EditorSceneManager.SaveScene(currentScene, fullPath, false);
            Debug.Log($"Scene saved to {fullPath}");
        }
        else
        {
            Debug.LogWarning($"File at path {fullPath} already exists! Aborting save...");
        }
    }

    public static void LoadGame()
    {
        if (File.Exists(savePath))
        {
            // Load the saved scene
            byte[] sceneBytes = File.ReadAllBytes(savePath);
            Scene loadedScene = SceneManager.GetSceneByPath(savePath);
            if (loadedScene.isLoaded)
            {
                SceneManager.SetActiveScene(loadedScene);
            }
            else
            {
                SceneManager.LoadScene(savePath, LoadSceneMode.Single);
            }

            // Load other game data as necessary
            // ...

            Debug.Log("Game loaded.");
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
    }
}
