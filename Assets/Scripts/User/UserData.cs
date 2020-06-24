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
    public int _softCoins;
    public int _hardCoins;
    public int _unlockedCells;
    public string _username;
    public int[] _dinosaurs;
    public bool[] _tutorialCompleted;

    public UserData()
    {
        _ID = 0;

    }

    public UserData(string lastUpdTime)
    {
        _lastUpdatedTime = lastUpdTime;
        _ID = 0;
        _experience = 0;
        _softCoins = 20000;
        _hardCoins = 0;
        _unlockedCells = 4;
        _username = "Mechanic";
        _dinosaurs = new int[15];
        _tutorialCompleted = new bool[6];
}
}
