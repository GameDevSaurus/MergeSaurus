using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenSpin()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseSpin()
    {
        _mainPanel.SetActive(false);
    }
}