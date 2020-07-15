using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

[ExecuteInEditMode]
public class EditorProjectManager : MonoBehaviour
{
    public static int _selectedProject = 0;
    static string[] _projectNames = {"PokeMerge", "MergeSaurus", "ChibiMerge"};

    [MenuItem("SwapProject/PokeMerge")]
    public static void SwapToPokeMerge()
    {
        SwapProject(0);
    }
    [MenuItem("SwapProject/MergeSaurus")]
    public static void SwapToMergeSaurus()
    {
        SwapProject(1);
    }
    [MenuItem("SwapProject/ChibiMerge")]
    public static void SwapToChibiMerge()
    {
        SwapProject(2);
    }
    public static void SwapProject(int projectIndex)
    {
        _selectedProject = projectIndex;
        PlayerSettings.productName = _projectNames[_selectedProject];
        PlayerSettings.applicationIdentifier = "com.KaijuBits." + _projectNames[_selectedProject];
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { Resources.Load<Texture2D>("Icons/" + projectIndex + "_gameIcon") });
    }
}
