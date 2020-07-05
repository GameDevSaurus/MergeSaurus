using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TimeController: MonoBehaviour
{
    /* 
          necessary variables to hold all the things we need.
        php url
        timedata, the data we get back
        current time
        current date
    */

    private string _url = "http://torcudev.es/thebeating/time.php";
    private double _currentTimestamp;
    DateTime _currentDatetime;
    DateTime _dateAtStart;
    float _elapsedSeconds;
    public bool _timeChecked;
    void Awake()
    {
        
    }
    
    public static DateTime ServerDateToDateTime(string phpDate)
    {
        string[] fullDate = phpDate.Split('/');
        string[] onlyDate = fullDate[0].Split('-');
        string[] onlyTime = fullDate[1].Split(':');
        DateTime dt = new DateTime(int.Parse(onlyDate[0]),int.Parse(onlyDate[1]),int.Parse(onlyDate[2]),int.Parse(onlyTime[0]),int.Parse(onlyTime[1]),int.Parse(onlyTime[2]));
        return dt;
    }
    public IEnumerator GetTime()
    {
        WWW www = new WWW(_url);
        yield return www;
        _dateAtStart = ServerDateToDateTime(www.text);
        _timeChecked = true;
    }
    public DateTime GetTimeNow()
    {
        return _dateAtStart.AddSeconds(_elapsedSeconds);
    }
    void Start()
    {
        StartCoroutine(GetTime());
    }
    private void Update()
    {
        if (_timeChecked)
        {
            _elapsedSeconds += Time.deltaTime;
        }
    }
}