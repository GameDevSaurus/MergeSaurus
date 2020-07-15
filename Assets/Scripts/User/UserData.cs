using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class UserData 
{
    public int _ID;
    public string _lastUpdatedTime;
    public int _experience;
    public int _level;
    public int[] _softCoins;
    public int _hardCoins;
    public int _unlockedCells;
    public int _unlockedExpositors;
    public string _username;
    public int[] _dinosaurs;
    public int[] _workingCellsByExpositor;
    public bool[] _tutorialCompleted;
    public int[] _purchasedTimes;
    public int[] _obtainedTimes;
    public bool[] _claimedAchievements;
    public int _discountLevel;
    public int _extraEarningsLevel;
    public int _extraTouristSpeedLevel;
    public int _extraPasiveEarningsLevel;
    public int _spinRemainingAds;
    public string _spinLastViewedAd;
    public int _speedUpRemainingSecs;
    public string _SpeedUpLastUse;
    public string _dayCareLastViewedAd;
    public int _biggestDino;
    public string _dailyRewardCheck;
    public int _playedDays;
    public int _dailyMerges;
    public int _dailyMergeLevel;
    public int _dailyAds;
    public int _dailyAdLevel;
    public int _dailyPurchases;
    public int _dailyPurchaseLevel;

    public UserData()
    {
        _lastUpdatedTime = System.DateTime.Now.ToBinary().ToString();
        _ID = 0;
        _experience = 0;
        _level = 1;
        _softCoins = new int[31];
        _softCoins[0] = 20000;
        _hardCoins = 0;
        _unlockedCells = 4;
        _unlockedExpositors = 4;
        _username = "Mechanic";
        _dinosaurs = new int[21];
        _workingCellsByExpositor = new int[10];
        _discountLevel = 0;
        _extraEarningsLevel = 0;
        _extraTouristSpeedLevel = 0;
        _extraPasiveEarningsLevel = 0;
        _spinRemainingAds = 3;
        _spinLastViewedAd = System.DateTime.Now.ToBinary().ToString();
        _speedUpRemainingSecs = 0;
        _SpeedUpLastUse = System.DateTime.Now.ToBinary().ToString();
        _dayCareLastViewedAd = System.DateTime.Now.ToBinary().ToString();
        _dailyRewardCheck = System.DateTime.Now.ToBinary().ToString();
        _biggestDino = 0;
        _playedDays = 0;
        for (int i = 0; i<_dinosaurs.Length; i++)
        {
            _dinosaurs[i] = -1;
        }
        for (int i = 0; i < _workingCellsByExpositor.Length; i++)
        {
            _workingCellsByExpositor[i] = -1;
        }
        _purchasedTimes = new int[40]; //Numero de indices igual al de tipos de dinosaurio #x
        _obtainedTimes = new int[40]; //Numero de indices igual al de tipos de dinosaurio #x
        _tutorialCompleted = new bool[10];
        _claimedAchievements = new bool[10];
        _dailyMerges = 0;
        _dailyAds = 0;
        _dailyPurchases = 0;
    }
}