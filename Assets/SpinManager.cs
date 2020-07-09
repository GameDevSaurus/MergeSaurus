using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    int _currentTries = 3;

    enum SpinRewards {Boxes, SmallSpeedTime, BigSpeedTime, Money2H, Money4H};

    public void OpenSpin()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseSpin()
    {
        _mainPanel.SetActive(false);
    }
    public void ShowVideo()
    {
        if(_currentTries > 0)
        {
            _currentTries--;
            GameEvents.PlayAd.Invoke("SpinReward");
        }
    }
    
}