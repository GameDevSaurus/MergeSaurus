using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeController : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    PanelManager _panelManager;
    [SerializeField]
    GameObject _productPrefab;
    [SerializeField]
    GameObject _fullProductPanel;
    [SerializeField]
    Transform _specificProductParent;
    [SerializeField]
    Image _cellCategory, _expositorCategory, _groundCategory, _frameCategory;
    [SerializeField]
    GameObject _onePhotoCustomize, _frameCustomize;
    [SerializeField]
    CustomizeBigPhotoInstance _customizeBigPhoto, _framesBigPhoto;
    [SerializeField]
    GameObject _backButton;
    CellManager _cellManager;
    EconomyManager _economyManager;
    RewardManager _rewardManager;
    List<ProductInstance> _productInstances;
    List<int> _cellsCost = new List<int>() {0, 50, 50, 50, 50, 100};
    List<int> _expositorsCost = new List<int>() {0, 50, 50, 50, 100 };
    List<int> _groundsCost = new List<int>() {0, 50, 50, 50, 100 };
    List<int> _framesCost = new List<int>() {0, 50, 50, 50, 100 };
    List<List<int>> _costList;

    int _lastPanelOpenedIndex = 0;
    bool _isBigProductOpened = false;

    public void OpenCustomize()
    {
        _panelManager.RequestShowPanel(_mainPanel);
        _isBigProductOpened = false;
        _fullProductPanel.SetActive(true);
        _specificProductParent.gameObject.SetActive(false);
        _backButton.SetActive(false);
        _onePhotoCustomize.SetActive(false);
        _frameCustomize.SetActive(false);
        RefreshInitialIcons();
    }
    public void CloseCustomize()
    {
        _mainPanel.SetActive(false);
    }

    public void RefreshInitialIcons()
    {
        _cellCategory.sprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
        _cellCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Cells/" + UserDataController.GetCurrentCell());
        _expositorCategory.sprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
        _expositorCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Expositors/" + UserDataController.GetCurrentExpositor());
        _groundCategory.sprite = Resources.Load<Sprite>("Sprites/Grounds/" + UserDataController.GetCurrentGround());
        _groundCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Grounds/" + UserDataController.GetCurrentGround());
        _frameCategory.sprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        _frameCategory.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
    }
    private void Start()
    {
        _cellManager = FindObjectOfType<CellManager>();
        _panelManager = FindObjectOfType<PanelManager>();
        _productInstances = new List<ProductInstance>();
        _economyManager = FindObjectOfType<EconomyManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
        _costList = new List<List<int>>();
        _costList.Add(_cellsCost);
        _costList.Add(_expositorsCost);
        _costList.Add(_groundsCost);
        _costList.Add(_framesCost);
        for (int i = 0; i < 10; i++)
        {
            GameObject nProduct = Instantiate(_productPrefab, _specificProductParent);
            _productInstances.Add(nProduct.GetComponent<ProductInstance>());
        }
    }
    public void OpenSpecificPanel(int productIndex)
    {
        _isBigProductOpened = false;
        _fullProductPanel.SetActive(false);
        _specificProductParent.gameObject.SetActive(true);
        _backButton.SetActive(true);
        _lastPanelOpenedIndex = productIndex;
        for (int i = 0; i < _productInstances.Count; i++)
        {
            _productInstances[i].gameObject.SetActive(false);
        }
        RefreshProducts();
    }

    public int GetHardCoinsProductCost(int categoryIndex, int productIndex)
    {
        return _costList[categoryIndex][productIndex];
    }

    public List<List<int>> GetAllCostList()
    {
        return _costList;
    }

    public void RefreshProducts()
    {
        switch (_lastPanelOpenedIndex)
        {
            case 0:
                for (int i = 0; i < UserDataController.GetCellsSkins().Length; i++)
                {
                    _productInstances[i].gameObject.SetActive(true);
                    Sprite productSprite = Resources.Load<Sprite>("Sprites/Cells/" + i);
                    _productInstances[i].InitProduct(productSprite, _cellsCost[i], i, this, _lastPanelOpenedIndex);
                }
                break;
            case 1:
                for (int i = 0; i < UserDataController.GetExpositorsSkins().Length; i++)
                {
                    _productInstances[i].gameObject.SetActive(true);
                    Sprite productSprite = Resources.Load<Sprite>("Sprites/Expositors/" + i);
                    _productInstances[i].InitProduct(productSprite, _expositorsCost[i], i, this, _lastPanelOpenedIndex);
                }
                break;
            case 2:
                for (int i = 0; i < UserDataController.GetGroundSkins().Length; i++)
                {
                    _productInstances[i].gameObject.SetActive(true);
                    Sprite productSprite = Resources.Load<Sprite>("Sprites/Grounds/" + i);
                    _productInstances[i].InitProduct(productSprite, _groundsCost[i], i, this, _lastPanelOpenedIndex);
                }
                break;
            case 3:
                for (int i = 0; i < UserDataController.GetFramesSkins().Length; i++)
                {
                    _productInstances[i].gameObject.SetActive(true);
                    Sprite productSprite = Resources.Load<Sprite>("Sprites/Frames/" + i);
                    _productInstances[i].InitProduct(productSprite, _framesCost[i], i, this, _lastPanelOpenedIndex);
                }
                break;
        }
    }

    public void PurchaseProduct(int productIndex, int category, CustomizeBigPhotoInstance c)
    {
        if (_economyManager.SpendHardCoins(GetHardCoinsProductCost(category, productIndex)))
        {
            switch (category)
            {
                case 0:
                    UserDataController.UnlockCellSkin(productIndex);
                    break;
                case 1:
                    UserDataController.UnlockExpositorSkin(productIndex);
                    break;
                case 2:
                    UserDataController.UnlockGroundSkin(productIndex);
                    break;
                case 3:
                    UserDataController.UnlockFrameSkin(productIndex);
                    break;
            }
            _rewardManager.EarnCustomizeItem(productIndex, category);
        }
        c.LoadConfig(0);
    }

    public void SetProduct(int _category, int _productIndex)
    {
        switch (_category)
        {
            case 0:
                UserDataController.SetCurrentCell(_productIndex);
                break;
            case 1:
                UserDataController.SetCurrentExpositor(_productIndex);
                break;
            case 2:
                UserDataController.SetCurrentGround(_productIndex);
                break;
            case 3:
                UserDataController.SetCurrentFrame(_productIndex);
                break;
        }
        _cellManager.RefreshSprites();
    }

    public void OpenBigPanel(int productInstanceIndex)
    {
        _isBigProductOpened = true;
        _specificProductParent.gameObject.SetActive(false);
        if(_lastPanelOpenedIndex != 3)
        {
            _onePhotoCustomize.SetActive(true);
            _customizeBigPhoto.Init(productInstanceIndex, _lastPanelOpenedIndex);
        }

        else
        {
            _frameCustomize.SetActive(true);
            _framesBigPhoto.Init(productInstanceIndex, _lastPanelOpenedIndex);
        }
    }

    public void Back()
    {
        if (!_isBigProductOpened)
        {
            _isBigProductOpened = false;
            _fullProductPanel.SetActive(true);
            _specificProductParent.gameObject.SetActive(false);
            _backButton.SetActive(false);
            _specificProductParent.localPosition = new Vector3(_specificProductParent.localPosition.x, 0, _specificProductParent.localPosition.z);
            RefreshInitialIcons();
        }
        else
        {
            OpenSpecificPanel(_lastPanelOpenedIndex);
            _onePhotoCustomize.SetActive(false);
            _frameCustomize.SetActive(false);
        }
    }
}
