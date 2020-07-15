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
    public static int lastSaveTime = -1;
    public static void Initialize()
    {
        _currentUserData = new UserData();
        SaveToFile();
        _checked = true;
    }
    public static DateTime GetLastSave()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._lastUpdatedTime));
    }
    public static DateTime GetLastPlayedDay()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._dailyRewardCheck));
    }
    public static int GetSecondsSinceLastSave()
    {
        return lastSaveTime;
    }

    public static void InitializeUser()
    {
        _currentUserData = new UserData();
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
        if(lastSaveTime < 0)
        {
            DateTime lastUpdatedTime = DateTime.FromBinary(Convert.ToInt64(_currentUserData._lastUpdatedTime));
            DateTime now = System.DateTime.Now;
            TimeSpan elapsedTime = now.Subtract(lastUpdatedTime);
            int elapsedSeconds = (int)elapsedTime.TotalSeconds;
            lastSaveTime = elapsedSeconds;
        }
        

        _currentUserData._lastUpdatedTime = System.DateTime.Now.ToBinary().ToString();
        File.WriteAllText(Application.persistentDataPath + "/" + _fileName, JsonUtility.ToJson(_currentUserData));
    }
    private void Start()
    {
        lastSaveTime = -1;
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

    public static void BuyDinosaur(int dinosaurIndex)
    {
        for (int i = 0; i < _currentUserData._unlockedCells; i++)
        {
            if (_currentUserData._dinosaurs[i] == -1)
            {
                _currentUserData._dinosaurs[i] = dinosaurIndex;
                break;
            }
        }
        _currentUserData._purchasedTimes[dinosaurIndex]++;
        _currentUserData._obtainedTimes[dinosaurIndex]++;
        SaveToFile();
    }

    public static void CreateDinosaur(int targetCell, int dinosaurIndex) 
    {
        _currentUserData._dinosaurs[targetCell] = dinosaurIndex;
        SaveToFile();
    }
    public static void CreateBox(BoxManager.BoxType boxType,  int targetCell, int boxIndex)
    {
        int sum = 0;
        switch (boxType)
        {
            case BoxManager.BoxType.StandardBox:
                sum = 100;
                break;
            case BoxManager.BoxType.RewardedBox:
                sum = 200;
                break;
            case BoxManager.BoxType.LootBox:
                sum = 300;
                break;
        }
        _currentUserData._dinosaurs[targetCell] = boxIndex + sum;
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
            SetBiggestDino(mergeDinoType + 1);
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

    public static int GetObtainedDinosByDinotype(int dinoType)
    {
        return _currentUserData._obtainedTimes[dinoType];
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
    public static void SpendHardCoins(int amount)
    {
        _currentUserData._hardCoins -= amount;
        SaveToFile();
    }
    public static void SpendSoftCoins(GameCurrency amount)
    {
        GameCurrency g = new GameCurrency(_currentUserData._softCoins);
        g.SubstractCurrency(amount);
        _currentUserData._softCoins = g.GetIntList();
        SaveToFile();
    }
    public static int GetHardCoins()
    {
        return _currentUserData._hardCoins;
    }

    public static int GetOwnedDinosByDinoType(int dinoType)
    {
        return _currentUserData._purchasedTimes[dinoType];
    }

    public static int GetBiggestDino()
    {
        return _currentUserData._biggestDino;
    }
    public static void SetBiggestDino(int biggestDino)
    {
        _currentUserData._biggestDino = biggestDino;
        SaveToFile();
    }
    public static int GetDiscountUpgradeLevel()
    {
        return _currentUserData._discountLevel;
    }
    public static int GetExtraEarningsLevel()
    {
        return _currentUserData._extraEarningsLevel;
    }
    public static int GetExtraTouristSpeedLevel()
    {
        return _currentUserData._extraTouristSpeedLevel;
    }
    public static int GetExtraPassiveEarningsLevel()
    {
        return _currentUserData._extraPasiveEarningsLevel;
    }

    public static void LevelUpUpgrade(UpgradesManager.UpgradeTypes upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradesManager.UpgradeTypes.Discount:
                _currentUserData._discountLevel++;
                break;
            case UpgradesManager.UpgradeTypes.DinoEarnings:
                _currentUserData._extraEarningsLevel++;
                break;
            case UpgradesManager.UpgradeTypes.TouristSpeed:
                _currentUserData._extraTouristSpeedLevel++;
                break;
            case UpgradesManager.UpgradeTypes.PassiveEarnings:
                _currentUserData._extraPasiveEarningsLevel++;
                break;
        }
    }

    public void SpeedUp(int secs)
    {
        _currentUserData._speedUpRemainingSecs = secs;
        _currentUserData._SpeedUpLastUse = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }
    public static DateTime GetLastSpeedUpTime()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._SpeedUpLastUse));
    }
    public static int GetSpeedUpRemainingSecs()
    {
        return _currentUserData._speedUpRemainingSecs;
    }
    public static void UpdateSpeedUpData(int remainingSecs)
    {
        _currentUserData._speedUpRemainingSecs = remainingSecs;
        _currentUserData._SpeedUpLastUse = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }

    public static int GetSpinRemainingAds()
    {
        return _currentUserData._spinRemainingAds;
    }
    public static DateTime GetSpinLastViewTime()
    {
        return DateTime.FromBinary(Convert.ToInt64(_currentUserData._spinLastViewedAd));
    }
    public static void UpdateSpinData(int remainingAds)
    {
        _currentUserData._spinRemainingAds = remainingAds;
        _currentUserData._spinLastViewedAd = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }
    public static GameCurrency GetSoftCoins()
    {
        return new GameCurrency(_currentUserData._softCoins);
    }

    public static int GetDinoAmountByType(int dinoType)
    {
        int amount = 0;
        for(int i = 0; i<_currentUserData._dinosaurs.Length; i++)
        {
            if(_currentUserData._dinosaurs[i] == dinoType)
            {
                amount++;
            }
        }
        return amount;
    }
    public static List<int> GetFirstThreeDinosByType(int dinoType)
    {
        List<int> threeFirstDinos = new List<int>();

        for (int i = 0; i < _currentUserData._dinosaurs.Length; i++)
        {
            if (_currentUserData._dinosaurs[i] == dinoType)
            {
                threeFirstDinos.Add(i);
                if (threeFirstDinos.Count > 2)
                {
                    break;
                }
            }
        }
        return threeFirstDinos;
    }
    public static void AddPlayedDay()
    {
        _currentUserData._playedDays++;
        _currentUserData._dailyRewardCheck = System.DateTime.Now.ToBinary().ToString();
        SaveToFile();
    }
    public static int GetPlayedDays()
    {
        return _currentUserData._playedDays % 7;
    }
    public static void ClaimAchievement(int achievementID)
    {
        _currentUserData._claimedAchievements[achievementID] = true;
        SaveToFile();
    }
    public static bool GetClaimedAchievement(int index)
    {
        return _currentUserData._claimedAchievements[index];
    }

    //DAILYMISSIONS MERGES
    public static int GetDailyMerges()
    {
        return _currentUserData._dailyMerges;
    }
    public static int GetDailyMergeLevel()
    {
        return _currentUserData._dailyMergeLevel;
    }
    public static void AddDailyMergeLevel()
    {
        _currentUserData._dailyMerges = 0;
        _currentUserData._dailyMergeLevel ++;
    }
    public static void AddDailyMerge()
    {
        _currentUserData._dailyMerges++;
    }

    //DAILYMISSIONS ADDS
    public static int GetDailyAds()
    {
        return _currentUserData._dailyAds;
    }
    public static int GetDailyAdLevel()
    {
        return _currentUserData._dailyAdLevel;
    }
    public static void AddDailyAdLevel()
    {
        _currentUserData._dailyAds = 0;
        _currentUserData._dailyAdLevel++;
    }
    public static void AddDailyAd()
    {
        _currentUserData._dailyAds++;
    }

    //DAILYMISSIONS PURCHASES
    public static int GetDailyPurchases()
    {
        return _currentUserData._dailyPurchases;
    }
    public static int GetDailyPurchaseLevel()
    {
        return _currentUserData._dailyPurchaseLevel;
    }
    public static void AddDailyPurchaseLevel()
    {
        _currentUserData._dailyPurchases = 0;
        _currentUserData._dailyPurchaseLevel++;
    }
    public static void AddDailyPurchase()
    {
        _currentUserData._dailyPurchases++;
    }

    public static void RestoreDailyMissions()
    {
        _currentUserData._dailyMerges = 0;
        _currentUserData._dailyMergeLevel = 0;
        _currentUserData._dailyAds = 0;
        _currentUserData._dailyAdLevel = 0;
        _currentUserData._dailyPurchases = 0;
        _currentUserData._dailyPurchaseLevel = 0;
    }
}
