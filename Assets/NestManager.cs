using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenNest()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseNest()
    {
        _mainPanel.SetActive(false);
    }
}