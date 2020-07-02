using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpManager : MonoBehaviour
{
    [SerializeField]
    GameObject _speedUpMain;

    public void OpenSpeedUpPanel()
    {
        _speedUpMain.SetActive(true);
    }
    public void CloseSpeedUpPanel()
    {
        _speedUpMain.SetActive(false);
    }
}
