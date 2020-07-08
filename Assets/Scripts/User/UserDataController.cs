using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Analytics;

public class UserDataController : MonoBehaviour
{

    public static UserData _currentUserData;
    public static string _fileName = "CurrentUserData.json";
    public static bool _checked;
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
        if (TimeController._timeChecked)
        {
            DateTime lastUpdatedTime = DateTime.FromBinary(Convert.ToInt64(_currentUserData._lastUpdatedTime));
            DateTime now = TimeController.GetTimeNow();
            TimeSpan elapsedTime = now.Subtract(lastUpdatedTime);
            int elapsedSeconds = (int)elapsedTime.TotalSeconds;

            /*
             * TO DO: elapsedSeconds > 300 --> Mostrar panel ganancias pasivas
             * 
             */

            _currentUserData._lastUpdatedTime = TimeController.GetTimeNow().ToBinary().ToString();
            File.WriteAllText(Application.persistentDataPath + "/" + _fileName, JsonUtility.ToJson(_currentUserData));
        }
    }

    public static bool Exist()
    {
        return File.Exists(Application.persistentDataPath + "/" + _fileName);
    }
    
    public static void AddCell()
    {
        _currentUserData._unlockedCells ++;
        SaveToFile();
    }
    public static void AddExpositor()
    {
        _currentUserData._unlockedExpositors++;
        SaveToFile();
    }

    public static float GetExperienceAmount()
    {
        CalculateLevel();
        float amount;
        float baseExp = ExperienceManager.experienceCost[UserDataController.GetLevel() - 1];
        float targetExp = ExperienceManager.experienceCost[UserDataController.GetLevel()];
        float currentExp = _currentUserData._experience;
        float diff = targetExp - baseExp;
        float currentDiff = currentExp - baseExp;
        amount = currentDiff / diff;
        return amount;
    }

    public static void AddExperiencePoints(int expAmount)
    {
        CalculateLevel();
        int preLevel = _currentUserData._level;
        _currentUserData._experience += expAmount;
        CalculateLevel();
        int postLevel = _currentUserData._level;
        if(preLevel < postLevel)
        {
            GameEvents.LevelUp.Invoke(postLevel);
        }
    }

    public static bool IsGoingToLvlUp(int expAmount)
    {
        int preLevel = _currentUserData._level;
        int level = 0;
        do
        {
            level++;
        }
        while ((_currentUserData._experience+expAmount) >= ExperienceManager.experienceCost[level]);

        if (level > _currentUserData._level)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void CalculateLevel()
    {
        int currentUserDataLevel = _currentUserData._level;
        int level = 0;
        do
        {
            level++;
        }
        while (_currentUserData._experience >= ExperienceManager.experienceCost[level]);
        if (currentUserDataLevel != level)
        {
            _currentUserData._level = level;
            SaveToFile();
        }
        
    }
    public static int GetLevel()
    {
        return _currentUserData._level;
    }

    public static void SaveTutorial(int tutorialIndex)
    {
        _currentUserData._tutorialCompleted[tutorialIndex] = true;
        SaveToFile();
    }

    public static void BuyDinosaur(int dinosaurIndex, GameCurrency cost)
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
        _currentUserData._obtainedTimes[dinosaurIndex]++;
        GameCurrency gCToSubstract = new GameCurrency((_currentUserData._softCoins));
        gCToSubstract.SubstractCurrency(cost);
        _currentUserData._softCoins = gCToSubstract.GetIntList();
        SaveToFile();
    }

    public static void CreateDinosaur(int targetCell, int dinosaurIndex) 
    {
        _currentUserData._dinosaurs[targetCell] = dinosaurIndex;
        SaveToFile();
    }
    public static void CreateBox(int targetCell, int boxIndex)
    {
        _currentUserData._dinosaurs[targetCell] = boxIndex + 100;
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
    public static int GetEmptyExpositors()
    {
        int unlockedExpositors = 0;
        for (int i = 0; i < _currentUserData._unlockedExpositors; i++)
        {
            if (_currentUserData._workingCellsByExpositor[i] == -1)
            {
                unlockedExpositors++;
            }
        }
        return unlockedExpositors;
    }

    public static bool HaveMoney(GameCurrency cost)
    {
        bool haveMoney = false;
        if(new GameCurrency(_currentUserData._softCoins).CompareCurrencies(cost))
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

    public static void DeleteDino(int dinocell)
    {
        _currentUserData._dinosaurs[dinocell] = -1;
        SaveToFile();
    }

    public static bool MergeDinosaurs(int originCell, int targetCell, int mergeDinoType)
    {
        bool mergeUp = false;
        if((mergeDinoType + 1) > GetBiggestDino())
        {
            GameEvents.DinoUp.Invoke(mergeDinoType + 1);
            mergeUp = true;
        }
        _currentUserData._obtainedTimes[mergeDinoType + 1]++;
        _currentUserData._dinosaurs[originCell] = -1;
        _currentUserData._dinosaurs[targetCell] = mergeDinoType +1;
        SaveToFile();
        return mergeUp;
    }
    public static void ShowCell(int cell, int expositor)
    {
        _currentUserData._workingCellsByExpositor[expositor] = cell;
        SaveToFile();
    }
    public static void StopShowCell(int expositor)
    {
        _currentUserData._workingCellsByExpositor[expositor] = -1;
        SaveToFile();
    }

    public static int GetExpositorIndexByCell(int cell)
    {
        int foundedExpositor = -1;
        for (int i = 0; i < _currentUserData._workingCellsByExpositor.Length; i++)
        {
            if(_currentUserData._workingCellsByExpositor[i] == cell)
            {
                foundedExpositor = i;
                break;
            }
        }
        return foundedExpositor;
    }
    public static void AddSoftCoins(GameCurrency softCoins)
    {
        GameCurrency gCToAdd = new GameCurrency((_currentUserData._softCoins));
        gCToAdd.AddCurrency(softCoins);
        _currentUserData._softCoins = gCToAdd.GetIntList();
        SaveToFile();
    }

    public static void AddHardCoins(int amount)
    {
        _currentUserData._hardCoins += amount;
        SaveToFile();
    }

    public static int GetOwnedDinosByDinoType(int dinoType)
    {
        return _currentUserData._purchasedTimes[dinoType];
    }

    public static int GetBiggestDino()
    {
        int biggestDino = 0;
        for (int i = 0; i <_currentUserData._dinosaurs.Length; i++)
        {
            if (_currentUserData._dinosaurs[i] < 100)
            {
                if (_currentUserData._dinosaurs[i] > biggestDino)
                {
                    biggestDino = _currentUserData._dinosaurs[i];
                }
            }
        }
        return biggestDino;
    }


}
