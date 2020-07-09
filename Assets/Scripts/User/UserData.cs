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
    int _discountLevel;
    int _extraEarningsLevel;
    int _extraSpeedLevel;
    int _extraRobotSpeed;


    public UserData()
    {
        _ID = 0;
    }

    public UserData(string lastUpdTime)
    {
        _lastUpdatedTime = lastUpdTime;
        _ID = 0;
        _experience = 0;
        _level = 1;
        _softCoins = new int[31];
        _softCoins[0] = 20000;
        _hardCoins = 0;
        _unlockedCells = 4;
        _unlockedExpositors = 4;
        _username = "Mechanic";
        _dinosaurs = new int[16];
        _workingCellsByExpositor = new int[10];
        _discountLevel = 0;
        _extraEarningsLevel = 0;
        _extraSpeedLevel = 0;
        _extraRobotSpeed = 0;
        for (int i = 0; i<_dinosaurs.Length; i++)
        {
            _dinosaurs[i] = -1;
        }
        for (int i = 0; i < _workingCellsByExpositor.Length; i++)
        {
            _workingCellsByExpositor[i] = -1;
        }
        _purchasedTimes = new int[16]; //Numero de indices igual al de tipos de dinosaurio #x
        _obtainedTimes = new int[16]; //Numero de indices igual al de tipos de dinosaurio #x
        _tutorialCompleted = new bool[10];
    }
}
