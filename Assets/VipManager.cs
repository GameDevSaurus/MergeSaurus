using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VipManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenVip()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseVip()
    {
        _mainPanel.SetActive(false);
    }
}