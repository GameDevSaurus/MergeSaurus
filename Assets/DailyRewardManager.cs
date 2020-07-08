using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenDaily()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseDaily()
    {
        _mainPanel.SetActive(false);
    }
}