using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class EditorTools : MonoBehaviour
{


    [MenuItem("Scenes/Configuration")]
    public static void OpenConfiguration()
    {
        LoadScene("Configuration");
    }

    [MenuItem("Scenes/MainGame")]
    public static void OpenMainGame()
    {
        LoadScene("MainGame");
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
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    [MenuItem("CustomTools/Delete persistentDataPath Folder")]
    [SerializeField]
    public static void DeleteStreamingAssetsFolder()
    {
        File.Delete(Application.persistentDataPath + "/CurrentUserData.json");
    }


    public static void LoadScene(string name)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) //Si el usuario quiere guardar la escena, guardar
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }

}
