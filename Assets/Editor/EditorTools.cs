using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorTools : MonoBehaviour
{
    [MenuItem("Scenes/Splash")]
    public static void OpenSplash()
    {
        LoadScene("Splash");
    }

    [MenuItem("Scenes/Configuration")]
    public static void OpenConfiguration()
    {
        LoadScene("Configuration");
    }

    [MenuItem("Scenes/MainMenu")]
    public static void OpenMainMenu()
    {
        LoadScene("MainMenu");
    }

    [MenuItem("CustomTools/DeletePlayerPrefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    [MenuItem("CustomTools/Open persistentDataPath Folder")]
    [SerializeField]
    public static void OpenStreamingAssetsFolder()
    {
        print(Application.streamingAssetsPath);
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    public static void LoadScene(string name)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) //Si el usuario quiere guardar la escena, guardar
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }

}
