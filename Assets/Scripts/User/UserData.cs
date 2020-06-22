using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class UserData 
{
    public int _ID;
    public string _lastUpdatedTime;

    public UserData()
    {
        _ID = 0;

    }

    public UserData(string lastUpdTime)
    {
        _lastUpdatedTime = lastUpdTime;
    }
}
