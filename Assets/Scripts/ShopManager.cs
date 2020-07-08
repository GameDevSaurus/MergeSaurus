using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;

    public void OpenShop()
    {
        _mainPanel.SetActive(true);
    }
    public void CloseShop()
    {
        _mainPanel.SetActive(false);
    }
}
