using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class UserDataController : MonoBehaviour
{

    public static UserData _currentUserData;
    public static string _fileName = "CurrentUserData.json";
    public static bool _checked;
    public static int[] _levelProgression = new int[]{ 0, 50, 75, 150 };

    public static void Initialize()
    {
        _currentUserData = new UserData();
        SaveToFile();
        _checked = true;
    }

    public static void InitializeUser(string lastUpdateTime)
    {
        _currentUserData = new UserData(lastUpdateTime);
        SaveToFile();
        _checked = true;
    }

    public static void SetUserData()
    {
        if (_currentUserData == null)
        {
            Initialize();
        }

        SaveToFile();
    }
    public static void LoadFromFile()
    {
        _currentUserData = JsonUtility.FromJson<UserData>(File.ReadAllText(Application.persistentDataPath + "/" + _fileName));
        _checked = true;

    }
    public static void DeleteFile()
    {
        File.Delete(Application.persistentDataPath + "/" + _fileName);
    }

    public static void SaveToFile()
    {
        File.WriteAllText(Application.persistentDataPath + "/" + _fileName, JsonUtility.ToJson(_currentUserData));
    }

    public static bool Exist()
    {
        return File.Exists(Application.persistentDataPath + "/" + _fileName);
    }
    public void SaveFromServer()
    {

    }

    public void SaveToServer()
    {

    }
    public static int GetLevel()
    {
        int level = 0;
        do
        {
            level++;
        }
        while (_currentUserData._experience > _levelProgression[level]);
        return level;
    }

    public static void SaveTutorial(int tutorialIndex)
    {
        _currentUserData._tutorialCompleted[tutorialIndex] = true;
        SaveToFile();
    }

    public static void BuyDinosaur(int dinosaurIndex, int cost)
    {
        for(int i = 0; i< _currentUserData._unlockedCells; i++)
        {
            if (_currentUserData._dinosaurs[i] == -1)
            {
                _currentUserData._dinosaurs[i] = dinosaurIndex;
                break;
            }
        }
        _currentUserData._purchasedTimes[dinosaurIndex]++;
        _currentUserData._softCoins -= cost;
        SaveToFile();
    }

    public static int GetEmptyCells()
    {
        int unlockedCells = 0;
        for(int i = 0; i<_currentUserData._unlockedCells; i++)
        {
            if(_currentUserData._dinosaurs[i] == -1)
            {
                unlockedCells++;
            }
        }
        return unlockedCells;
    }

    public static bool HaveMoney(int cost)
    {
        bool haveMoney = false;
        if(_currentUserData._softCoins >= cost)
        {
            haveMoney = true;
        }
        return haveMoney;
    }

    public static void MoveDinosaur(int cellIndex1, int cellIndex2)
    {
        int aux = _currentUserData._dinosaurs[cellIndex2];
        _currentUserData._dinosaurs[cellIndex2] = _currentUserData._dinosaurs[cellIndex1];
        _currentUserData._dinosaurs[cellIndex1] = aux;
        SaveToFile();
    }

    public static void MergeDinosaurs(int originCell, int targetCell, int mergeDinoType)
    {
        _currentUserData._dinosaurs[originCell] = -1;
        _currentUserData._dinosaurs[targetCell] = mergeDinoType +1;
        SaveToFile();
    }
    public static void ShowCell(int cell, int expositor)
    {
        _currentUserData._workingCellsByExpositor[cell] = expositor;
        SaveToFile();
    }
}
