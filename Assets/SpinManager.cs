using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    enum SpinRewards {Boxes, SmallSpeedTime, BigSpeedTime, Money2H, Money4H};

    public void OpenSpin()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseSpin()
    {
        _mainPanel.SetActive(false);
    }

    
}