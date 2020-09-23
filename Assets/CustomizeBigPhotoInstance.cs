using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeBigPhotoInstance : MonoBehaviour
{
    [SerializeField]
    GameObject _hardCoinsButton, _selectButton, _selectedInfo;
    [SerializeField]
    TextMeshProUGUI _hardCoinsText;
    [SerializeField]
    CustomizeController _customizeController;
    [SerializeField]
    Image _background;
    [SerializeField]
    Image[] _seats;
    [SerializeField]
    Image[] _tables;
    [SerializeField]
    GameObject _leftButton, _rigthButton;
    [SerializeField]
    Image _frameImage;
    int _productIndex;
    int _category;
    List<int> _customizeDataList;

    public void Init(int productIndex, int categoryIndex)
    {
        _customizeDataList = new List<int>();
        _customizeDataList.Add(UserDataController.GetCellsSkins().Length);
        _customizeDataList.Add(UserDataController.GetExpositorsSkins().Length);
        _customizeDataList.Add(UserDataController.GetGroundSkins().Length);
        _customizeDataList.Add(UserDataController.GetFramesSkins().Length);
        _productIndex = productIndex;
        _category = categoryIndex;
        Button _hardCoins = _hardCoinsButton.GetComponent<Button>();
        //_hardCoins.onClick.RemoveAllListeners();
        //_hardCoins.onClick.AddListener(() => UnlockSkinHardCoins());
        _hardCoinsText.text = _customizeController.GetHardCoinsProductCost(_category , _productIndex).ToString();
        LoadConfig(0);
    }

    public void LoadConfig(int sum)
    {
        _productIndex += sum;
        _productIndex = Mathf.Clamp(_productIndex, 0, (_customizeController.GetAllCostList()[_category].Count - 1));
        _hardCoinsButton.SetActive(false);
        _selectedInfo.SetActive(false);
        _selectButton.SetActive(false);
        _hardCoinsText.text = _customizeController.GetHardCoinsProductCost(_category, _productIndex).ToString();

        if(_category != 3)
        {
            _background.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + UserDataController.GetCurrentGround());
            _background.overrideSprite = Resources.Load<Sprite>("Sprites/Grounds/" + UserDataController.GetCurrentGround());

            for (int i = 0; i < _seats.Length; i++)
            {
                _seats[i].sprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
                _seats[i].overrideSprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
            }
            for (int i = 0; i < _tables.Length; i++)
            {
                _tables[i].sprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
                _tables[i].overrideSprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
            }

            switch (_category)
            {
                case 0:
                    for (int i = 0; i < _seats.Length; i++)
                    {
                        _seats[i].sprite = Resources.Load<Sprite>("Sprites/Cells/" + _productIndex);
                        _seats[i].overrideSprite = Resources.Load<Sprite>("Sprites/Cells/" + _productIndex);
                    }
                    if (UserDataController.GetCellsSkins()[_productIndex])
                    {
                        if (UserDataController.GetCurrentCell() == _productIndex)
                        {
                            _selectedInfo.SetActive(true);
                        }
                        else
                        {
                            _selectButton.SetActive(true);
                        }
                    }
                    else
                    {
                        _hardCoinsButton.SetActive(true);
                    }
                    break;
                case 1:
                    for (int i = 0; i < _tables.Length; i++)
                    {
                        _tables[i].sprite = Resources.Load<Sprite>("Sprites/Expositors/" + _productIndex);
                        _tables[i].overrideSprite = Resources.Load<Sprite>("Sprites/Expositors/" + _productIndex);
                    }
                    if (UserDataController.GetExpositorsSkins()[_productIndex])
                    {
                        if (UserDataController.GetCurrentExpositor() == _productIndex)
                        {
                            _selectedInfo.SetActive(true);
                        }
                        else
                        {
                            _selectButton.SetActive(true);
                        }
                    }
                    else
                    {
                        _hardCoinsButton.SetActive(true);
                    }
                    break;
                case 2:
                    _background.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + _productIndex);
                    _background.overrideSprite = Resources.Load<Sprite>("Sprites/Grounds/" + _productIndex);
                    if (UserDataController.GetGroundSkins()[_productIndex])
                    {
                        if (UserDataController.GetCurrentGround() == _productIndex)
                        {
                            _selectedInfo.SetActive(true);
                        }
                        else
                        {
                            _selectButton.SetActive(true);
                        }
                    }
                    else
                    {
                        _hardCoinsButton.SetActive(true);
                    }
                    break;
            }
        }
        else
        {
            _frameImage.sprite = Resources.Load<Sprite>("Sprites/Frames/" + _productIndex);
            _frameImage.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + _productIndex);

            if (UserDataController.GetFramesSkins()[_productIndex])
            {
                if (UserDataController.GetCurrentFrame() == _productIndex)
                {
                    _selectedInfo.SetActive(true);
                }
                else
                {
                    _selectButton.SetActive(true);
                }
            }
            else
            {
                _hardCoinsButton.SetActive(true);
            }

        }

        if ((_customizeDataList[_category] - 1) == 0)
        {
            _leftButton.SetActive(false);
            _rigthButton.SetActive(false);
        }
        else
        {
            if (_productIndex > 0)
            {
                _leftButton.SetActive(true);
                if (_productIndex < (_customizeDataList[_category] - 1))
                {
                    _rigthButton.SetActive(true);
                }
                else
                {
                    _rigthButton.SetActive(false);
                }
            }
            else
            {
                _leftButton.SetActive(false);
            }
        }
    }

    public void SelectProduct()
    {
        _customizeController.SetProduct(_category, _productIndex);
        LoadConfig(0);
    }

    public void PurchaseProduct()
    {
        _customizeController.PurchaseProduct(_productIndex, _category, this);
    }
}
