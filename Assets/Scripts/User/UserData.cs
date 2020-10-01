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
    public int[] _totalSoftCoins;
    public int _hardCoins;
    public int _unlockedCells;
    public int _unlockedExpositors;
    public string _username;
    public int[] _dinosaurs;
    public bool[] _skins;
    public bool[] _specialCards;
    public int[] _workingCellsByExpositor;
    public bool[] _tutorialCompleted;
    public int[] _purchasedTimes;
    public int[] _obtainedTimes;
    public bool[] _claimedAchievements;
    public bool[] _achievementsToClaim;
    public bool[] _galleryImagesToOpen;
    public int _discountLevel;
    public int _currentCell;
    public bool[] _cellSkins;
    public bool[] _groundSkins;
    public bool[] _framesSkins;
    public int _currentExpositor;
    public int _currentGround;
    public int _currentFrame;
    public bool[] _expositorSkins;
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
    public int _dailySkinLevel;
    public int _dailyPurchases;
    public int _dailyPurchaseLevel;
    public int _playerAvatar;
    public int _currentRewardVideos;
    public int _freeSpinTries;
    public bool _missionWarning;
    public bool _specialOffer;
    public bool _vipUser;
    public bool _haswatchedGalleryTutorial;
    public bool _haswatchedGalleryTutorial2;
    public UserData()
    {
        _lastUpdatedTime = System.DateTime.Now.ToBinary().ToString();
        _ID = 0;
        _experience = 0;
        _level = 1;
        _softCoins = new int[31];
        _softCoins[0] = 7500;
        _totalSoftCoins = new int[31];
        _totalSoftCoins[0] = 20000;
        _hardCoins = 0; 
        _unlockedCells = 4;
        _unlockedExpositors = 4;
        _currentCell = 2;
        _currentExpositor = 0;
        _currentGround = 0;
        _currentFrame = 0;
        _cellSkins = new bool[5];
        _cellSkins[2] = true;
        _expositorSkins = new bool[5];
        _expositorSkins[0] = true;
        _groundSkins = new bool[5];
        _groundSkins[0] = true;
        _framesSkins = new bool[5];
        _framesSkins[0] = true;
        _username = "Player";
        _dinosaurs = new int[18];
        _skins = new bool[_dinosaurs.Length * 2];
        _skins[0] = true;
        _specialCards = new bool[_skins.Length];
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
        _currentRewardVideos = 0;
        _freeSpinTries = 1;
        _missionWarning = false;
        _specialOffer = false;
        _vipUser = false;

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
        _achievementsToClaim = new bool[_claimedAchievements.Length];
        _galleryImagesToOpen = new bool[_dinosaurs.Length*2];
        _galleryImagesToOpen[0] = true;
        _galleryImagesToOpen[1] = true;
        _dailyMerges = 0;
        _dailySkinLevel = 0;
        _dailyPurchases = 0;
        _playerAvatar = 0;
    }
}