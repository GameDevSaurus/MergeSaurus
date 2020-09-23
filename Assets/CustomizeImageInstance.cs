using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeImageInstance : MonoBehaviour
{
    [SerializeField]
    Image _productImage;
    [SerializeField]
    GameObject _openProductButton;
    CustomizeController _customizeController;

    private void Start()
    {
        _customizeController = FindObjectOfType<CustomizeController>();
    }
    public void OpenProductsList(int productIndex)
    {
        _customizeController.OpenSpecificPanel(productIndex);
    }
}
