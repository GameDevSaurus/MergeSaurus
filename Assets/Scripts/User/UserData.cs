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
    public int _unlockedExpositors;
    public string _username;
    public int[] _dinosaurs;
    public int[] _workingCellsByExpositor;
    public bool[] _tutorialCompleted;
    public int[] _purchasedTimes;

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
        _unlockedCells = 15; //Estamos iniciando al maximo
        _unlockedExpositors = 10;
        _username = "Mechanic";
        _dinosaurs = new int[15];
        _workingCellsByExpositor = new int[10];
        for (int i = 0; i<_dinosaurs.Length; i++)
        {
            _dinosaurs[i] = -1;
        }
        for (int i = 0; i < _workingCellsByExpositor.Length; i++)
        {
            _workingCellsByExpositor[i] = -1;
        }
        _purchasedTimes = new int[15]; //Numero de indices igual al de tipos de dinosaurio #x
        _tutorialCompleted = new bool[6];
    }
}
