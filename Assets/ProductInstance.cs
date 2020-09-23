using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductInstance : MonoBehaviour
{
    [SerializeField]
    Image _productImage;
    [SerializeField]
    GameObject _openFullScreenButton;
    [SerializeField]
    TextMeshProUGUI _hardCoinsCostPreview;
    int _productIndex;
    int _productCategory;
    CustomizeController _customizeController;
    [SerializeField]
    GameObject _lockedPanel;
    [SerializeField]
    GameObject _selectedPanel;
    [SerializeField]
    GameObject _hardCoinsPanel;
    [SerializeField]
    GameObject _useProductButton;

    public void InitProduct(Sprite productSprite, int gemCost, int productIndex, CustomizeController c, int productCategory)
    {
        _productImage.sprite = productSprite;
        _hardCoinsCostPreview.text = gemCost.ToString();
        _productIndex = productIndex;
        _productCategory = productCategory;
        _customizeController = c;
        _lockedPanel.SetActive(false);
        _hardCoinsPanel.SetActive(false);
        _selectedPanel.SetActive(false);
        _useProductButton.SetActive(false);
        switch (productCategory)
        {
            case 0:
                if (!UserDataController.GetCellsSkins()[productIndex])
                {
                    _lockedPanel.SetActive(true);
                    _hardCoinsPanel.SetActive(true);
                }
                else
                {
                    if (UserDataController.GetCurrentCell() == productIndex)
                    {
                        _selectedPanel.SetActive(true);
                    }
                    else
                    {
                        _useProductButton.SetActive(true);
                    }
                }
                break;
            case 1:
                if (!UserDataController.GetExpositorsSkins()[productIndex])
                {
                    _lockedPanel.SetActive(true);
                    _hardCoinsPanel.SetActive(true);
                }
                else
                {
                    if (UserDataController.GetCurrentExpositor() == productIndex)
                    {
                        _selectedPanel.SetActive(true);
                    }
                    else
                    {
                        _useProductButton.SetActive(true);
                    }
                }
                break;
            case 2:
                if (!UserDataController.GetGroundSkins()[productIndex])
                {
                    _lockedPanel.SetActive(true);
                    _hardCoinsPanel.SetActive(true);
                }
                else
                {
                    if (UserDataController.GetCurrentGround() == productIndex)
                    {
                        _selectedPanel.SetActive(true);
                    }
                    else
                    {
                        _useProductButton.SetActive(true);
                    }
                }
                break;
            case 3:
                if (!UserDataController.GetFramesSkins()[productIndex])
                {
                    _lockedPanel.SetActive(true);
                    _hardCoinsPanel.SetActive(true);
                }
                else
                {
                    if (UserDataController.GetCurrentFrame() == productIndex)
                    {
                        _selectedPanel.SetActive(true);
                    }
                    else
                    {
                        _useProductButton.SetActive(true);
                    }
                }
                break;
        }
    }

    public void OpenFullScreen()
    {
        _customizeController.OpenBigPanel(_productIndex);
    }

    public void UseProduct()
    {
        _customizeController.SetProduct(_productCategory, _productIndex);
        _customizeController.RefreshProducts();
    }
}
