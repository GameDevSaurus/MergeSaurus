using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GallerySinglePhotoInstance : MonoBehaviour
{
    [SerializeField]
    Image _mainImage;
    [SerializeField]
    GameObject _hardCoinsButton;
    [SerializeField]
    GameObject _softCoinsButton;
    [SerializeField]
    GameObject _lockIcon;
    [SerializeField]
    TextMeshProUGUI _softCoinsText;
    [SerializeField]
    TextMeshProUGUI _hardCoinsText;
    GalleryManager _galleryManager;
    [SerializeField]
    Image _frameImage;
    [SerializeField]
    GameObject _specialCardButton, _zoomIcon, _foreGround;
    [SerializeField]
    Image _background;
    int _myIndex;
    [SerializeField]
    CardConfigurator _cardConfigurator;
    public void Init(int characterIndex)
    {
        _myIndex = characterIndex;
        _galleryManager = FindObjectOfType<GalleryManager>();
        _cardConfigurator = GetComponent<CardConfigurator>();
        int biggestDino = UserDataController.GetBiggestDino() + 1;
        _cardConfigurator.Init(characterIndex);
        Button _softCoins = _softCoinsButton.GetComponent<Button>();
        Button _hardCoins = _hardCoinsButton.GetComponent<Button>();
        Button _specialCard = _specialCardButton.GetComponent<Button>();
        _softCoins.onClick.RemoveAllListeners();
        _hardCoins.onClick.RemoveAllListeners();
        _specialCard.onClick.RemoveAllListeners();
        _softCoins.onClick.AddListener(() =>UnlockSkinSoftCoins());
        _hardCoins.onClick.AddListener(() => UnlockSkinHardCoins());
        _specialCard.onClick.AddListener(() => UnlockSpecialCard());
        _softCoinsText.text = _galleryManager.GetSoftCostByIndex(characterIndex);
        _hardCoinsText.text = _galleryManager.GetHardCostByIndex(characterIndex);
        SetZoomButtonState(false);
        if (characterIndex <= (biggestDino * 2)-1)
        {
            if (UserDataController.IsSkinUnlocked(characterIndex))
            {
                UnlockAvailable();
                SetZoomButtonState(true);
            }
            else
            {
                UnlockUnavailable();
            }
        }
        else
        {
            Lock();
        }
        if (UserDataController.IsSpecialCardUnlocked(_myIndex))
        {
            _specialCardButton.SetActive(false);
        }
        else
        {
            _specialCardButton.SetActive(true);
        }
    }
    public void UnlockSkinSoftCoins()
    {
        if (_galleryManager.TryUnlockSkinSoftCoins(_myIndex, 2))
        {
            Init(_myIndex);
        }
    }
    public void UnlockSkinHardCoins()
    {
        if (_galleryManager.TryUnlockSkinHardCoins(_myIndex, 2))
        {
            Init(_myIndex);
        }
    }
    public void UnlockSpecialCard()
    {
        if (_galleryManager.TryUnlockSpecialCard(_myIndex))
        {
            Init(_myIndex);
        }
    }

    public void SetZoomButtonState(bool state)
    {
        _zoomIcon.SetActive(state);
        _frameImage.gameObject.SetActive(state);
        _foreGround.SetActive(state);
        _specialCardButton.SetActive(state);
        if (state)
        {
            if (UserDataController.IsSpecialCardUnlocked(_myIndex))
            {
                _specialCardButton.SetActive(false);
            }
            else
            {
                _specialCardButton.SetActive(true);
            }
        }
    }

    public void Lock()
    {
        _hardCoinsButton.SetActive(false);
        _softCoinsButton.SetActive(false);
        _mainImage.color = Color.black;
        _lockIcon.SetActive(true);
    }

    public void UnlockAvailable()
    {
        _lockIcon.SetActive(false);
        _mainImage.color = Color.white;
        _hardCoinsButton.SetActive(false);
        _softCoinsButton.SetActive(false);
    }

    public void UnlockUnavailable()
    {
        _lockIcon.SetActive(false);
        _hardCoinsButton.SetActive(true);
        _softCoinsButton.SetActive(true);
        _mainImage.color = Color.black;
    }
}
