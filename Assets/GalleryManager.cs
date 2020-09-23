using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GalleryManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    GameObject _warningIcon;
    PanelManager _panelManager;
    [SerializeField]
    GameObject _galleryHorizontalContainer;
    [SerializeField]
    GameObject _galleryImage;
    [SerializeField]
    RectTransform _gridWithoutElements;
    List<GalleryImageInstance> _galleryImages;
    [SerializeField]
    DayCareManager _dayCareManager;
    [SerializeField]
    GameObject _onePhotoGallery;
    [SerializeField]
    GameObject _fullGallery;
    [SerializeField]
    Image _onePhotoMainImage;
    int _currentOnePhotoIndex;
    [SerializeField]
    GameObject _backButton;
    [SerializeField]
    GameObject _previousButton;
    [SerializeField]
    GameObject _nextButton;
    [SerializeField]
    GameObject _hardCoinsButton;
    [SerializeField]
    GameObject _softCoinsButton;
    [SerializeField]
    GameObject _lockIcon;
    bool _onePhotoActive;
    Vector2 _positionFirstPinch;
    Vector2 _positionSecondPinch;
    float _pinchDistance;
    bool _pinching;
    Vector2 _originalSizeDelta;
    Camera _mainCamera;
    [SerializeField]
    RectTransform _rightImage;
    [SerializeField]
    RectTransform _leftImage;
    [SerializeField]
    RectTransform _centerImage;
    [SerializeField]
    GallerySinglePhotoInstance _instanceLeft;
    [SerializeField]
    GallerySinglePhotoInstance _instanceRight;
    [SerializeField]
    GallerySinglePhotoInstance _instanceCenter;
    [SerializeField]
    AnimationCurve _moveAnimationCurve;
    [SerializeField]
    TextMeshProUGUI _nameTx;
    List<GameCurrency> _softCoinPrizes;
    List<int> _hardCoinPrizes = new List<int>(){0, 0, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
    //CAMBIAR MAS ADELANTE 
    private void Awake()
    {
        GameCurrency _mekuCost =  new GameCurrency(new int[] {0,0,1,1});
        GameCurrency _mekuCostNude = new GameCurrency(new int[] { 0, 0, 1, 2 });
        GameCurrency _eriCost = new GameCurrency(new int[] { 0, 0, 1,3 });
        GameCurrency _eriCostNude = new GameCurrency(new int[] { 0, 0, 1, 4 });
        _softCoinPrizes = new List<GameCurrency>() { new GameCurrency(0), new GameCurrency(0), new GameCurrency(55000), new GameCurrency(115000), new GameCurrency(555000), new GameCurrency(1115000), new GameCurrency(5555000), new GameCurrency(11115000), new GameCurrency(55555000), new GameCurrency(111115000), new GameCurrency(211115000), new GameCurrency(311115000), new GameCurrency(411115000), new GameCurrency(511115000), new GameCurrency(_mekuCost.GetIntList()), new GameCurrency(_mekuCostNude.GetIntList()), new GameCurrency(_eriCost.GetIntList()), new GameCurrency(_eriCostNude.GetIntList()) };
        _mainCamera = Camera.main;
        _galleryImages = new List<GalleryImageInstance>();
        GameEvents.DinoUp.AddListener(UnlockDinoInGallery);
    }
    public void UnlockDinoInGallery(int dino)
    {
        int skinUnlocked = (dino * 2);
        UserDataController.UnlockSkin(skinUnlocked);
        RefreshAll();
    }
    public RectTransform GetGalleryImageSoftCoinsButton(int _galleryIndex)
    {
        return _galleryImages[_galleryIndex]._softCoinsButton.GetComponent<RectTransform>();
    }
    public void OpenGallery()
    {
        if (!UserDataController.HasSeenGalleryTutorial())
        {
            FindObjectOfType<Tutorial>().PlayGalleryTutorial();
        }
        else
        {
            if (!UserDataController.HasSeenGalleryTutorial2())
            {
                FindObjectOfType<Tutorial>().PlaySecondGalleryTutorial();
            }
        }
        RefreshAll();
        _onePhotoGallery.SetActive(false);
        _fullGallery.SetActive(true);
        _backButton.SetActive(false);
        _panelManager.RequestShowPanel(_mainPanel);
    }
    public void CloseGallery()
    {
        _mainPanel.SetActive(false);
        _panelManager.ClosePanel();
    }

    private void Start()
    {
        _panelManager = FindObjectOfType<PanelManager>();
        InitialCreation();
    }
    public void Refresh(int index)
    {
        _galleryImages[index].Init(index);
    }
    public void RefreshAll()
    {
        for (int i = 0; i < _galleryImages.Count; i++)
        {
            _galleryImages[i].Init(i);
        }
        RefreshWarningIcons();
    }

    public void InitialCreation()
    {
        int charsNumber = (UserDataController.GetDinoAmount() * 2) ;
        RectTransform horizontalParent = null;
        for (int i = 0; i < charsNumber; i++)
        {
            if (i % 2 == 0)
            {
                horizontalParent = Instantiate(_galleryHorizontalContainer, _gridWithoutElements).GetComponent<RectTransform>();
                GalleryImageInstance nGaleryInstance = Instantiate(_galleryImage, horizontalParent).GetComponent<GalleryImageInstance>();
                _galleryImages.Add(nGaleryInstance);
                nGaleryInstance.Init(i);
            }
            else
            {
                GalleryImageInstance nGaleryInstance = Instantiate(_galleryImage, horizontalParent).GetComponent<GalleryImageInstance>();
                _galleryImages.Add(nGaleryInstance);
                nGaleryInstance.Init(i);
            }
        }
    }
    public void ShowFullScreen(int characterIndex)
    {
        _currentOnePhotoIndex = characterIndex;
        _nameTx.text = GetPhotoName(_currentOnePhotoIndex);
        _previousButton.SetActive(true);
        if (_currentOnePhotoIndex == 0)
        {
            _previousButton.SetActive(false);
        }
        _instanceCenter.Init(_currentOnePhotoIndex);
        _onePhotoGallery.SetActive(true);
        _fullGallery.SetActive(false);
        _backButton.SetActive(true);
        _onePhotoActive = true;
        if (UserDataController.GetGalleryImagesToOpen()[_currentOnePhotoIndex])
        {
            UserDataController.SetGalleryImage(_currentOnePhotoIndex, false);
            RefreshAll();
        }
    }
    public void OnePhotoNext()
    {
        StartCoroutine(CrNextPhoto());
        //RefreshOnePhoto();
    }
    public bool TryUnlockSkinSoftCoins(int skin, int purchaseType)
    {
        bool canUnlockSkin = false;
        EconomyManager economyManager = FindObjectOfType<EconomyManager>();
        if (economyManager.SpendSoftCoins(_softCoinPrizes[(skin-1)/2]))
        {
            canUnlockSkin = true;
            UserDataController.UnlockSkin(skin);
            GameEvents.GetSkin.Invoke(new GameEvents.UnlockSkinEventData(skin, purchaseType));
            _panelManager.ClosePanel();
        }
        Refresh(skin);
        return canUnlockSkin;
    }
    public bool TryUnlockSkinHardCoins(int skin, int purchaseType)
    {
        bool canUnlockSkin = false;

        EconomyManager economyManager = FindObjectOfType<EconomyManager>();
        if (economyManager.SpendHardCoins(_hardCoinPrizes[(skin - 1) / 2])) //PRECIOS
        {
            UserDataController.UnlockSkin(skin);
            canUnlockSkin = true;
            GameEvents.GetSkin.Invoke(new GameEvents.UnlockSkinEventData(skin, purchaseType));
            _panelManager.ClosePanel();
        }
        
        Refresh(skin);
        return canUnlockSkin;
    }

    public bool TryUnlockSpecialCard(int skin)
    {
        bool canUnlockSkin = false;
        EconomyManager economyManager = FindObjectOfType<EconomyManager>();
        if (economyManager.SpendHardCoins(50))
        {
            UserDataController.UnlockSpecialCard(skin);
            canUnlockSkin = true;
            //LLAMAR AL REWARD
        }
        return canUnlockSkin;
    }

    IEnumerator CrNextPhoto()
    {
        _currentOnePhotoIndex++;
        _instanceRight.Init(_currentOnePhotoIndex);
        CheckButtons();
        StartCoroutine(PhotoNameSwap());
        for (float i = 0; i< 0.5f; i += Time.deltaTime)
        {
            _rightImage.anchoredPosition = Vector3.Lerp(new Vector3(2000,0,0), new Vector3(0, 0, 0),_moveAnimationCurve.Evaluate( i / 0.5f));
            _centerImage.anchoredPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(-2000, 0, 0), _moveAnimationCurve.Evaluate(i / 0.5f));
            yield return null;
        }

        _instanceCenter.Init(_currentOnePhotoIndex);
        //_instanceRight.Init(_currentOnePhotoIndex + 1);

        _rightImage.anchoredPosition = new Vector3(2000,0,0);
        _centerImage.anchoredPosition = Vector3.zero;
        if (UserDataController.GetGalleryImagesToOpen()[_currentOnePhotoIndex])
        {
            UserDataController.SetGalleryImage(_currentOnePhotoIndex, false);
            RefreshAll();
        }
    }
    IEnumerator CrPrevPhoto()
    {
        _currentOnePhotoIndex--;
        _instanceLeft.Init(_currentOnePhotoIndex);
        CheckButtons();
        StartCoroutine(PhotoNameSwap());
        for (float i = 0; i < 0.5f; i += Time.deltaTime)
        {
            _leftImage.anchoredPosition = Vector3.Lerp(new Vector3(-2000, 0, 0), new Vector3(0, 0, 0), _moveAnimationCurve.Evaluate(i / 0.5f));
            _centerImage.anchoredPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(2000, 0, 0), _moveAnimationCurve.Evaluate(i / 0.5f));
            yield return null;
        }
        _instanceCenter.Init(_currentOnePhotoIndex);
        //_instanceRight.Init(_currentOnePhotoIndex + 1);

        _leftImage.anchoredPosition = new Vector3(-2000, 0, 0);
        _centerImage.anchoredPosition = Vector3.zero;
        if (UserDataController.GetGalleryImagesToOpen()[_currentOnePhotoIndex])
        {
            UserDataController.SetGalleryImage(_currentOnePhotoIndex, false);
            RefreshAll();
        }
    }

    public string GetPhotoName(int index)
    {
        string photoName = _dayCareManager.GetNamesList()[(index / 4)];
        switch (index % 4)
        {
            case 1:
                photoName += " (Underwear)";
                break;
            case 2:
                photoName +=  " (Having Fun)";
                break;
            case 3:
                photoName += " (Explicit)";
                break;
        }
        return photoName;
    }
    public IEnumerator PhotoNameSwap()
    {
        float duration = 0.25f;
        Color alpha = new Color(0, 0, 0, 0);
        for (float i = 0; i<duration; i += Time.deltaTime)
        {
            _nameTx.color = Color.Lerp(Color.white, alpha, i / duration);
            yield return null;
        }
        _nameTx.color = alpha;
        _nameTx.text = GetPhotoName(_currentOnePhotoIndex);
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            _nameTx.color = Color.Lerp(alpha, Color.white, i / duration);
            yield return null;
        }
        _nameTx.color = Color.white; 
    }

    public void CheckButtons()
    {
        if (_currentOnePhotoIndex == UserDataController.GetChibiSkinsNumber()-1)
        {
            _nextButton.SetActive(false);
        }
        else
        {
            _nextButton.SetActive(true);
        }
        if (_currentOnePhotoIndex == 0)
        {
            _previousButton.SetActive(false);
        }
        else
        {
            _previousButton.SetActive(true);
        }
    }
    public void OnePhotoPrevious()
    {
        StartCoroutine(CrPrevPhoto());
        //RefreshPhoto();
    }

    public void OpenOneImage(int imageIndex)
    {
        OpenGallery();
        ShowFullScreen(imageIndex);
    }
    public void HideFullScreen()
    {
        _onePhotoGallery.SetActive(false);
        _fullGallery.SetActive(true);
        _backButton.SetActive(false);
        _onePhotoActive = false;
    }

    public string GetSoftCostByIndex(int index)
    {
        return _softCoinPrizes[(index - 1) / 2].GetCurrentMoneyConvertedTo3Chars();

    }
    public string GetHardCostByIndex(int index)
    {
        return _hardCoinPrizes[(index - 1) / 2].ToString();

    }

    public void RefreshWarningIcons()
    {
        _warningIcon.SetActive(false);
        for (int i = 0; i<UserDataController.GetGalleryImagesToOpen().Length; i++)
        {
            if (UserDataController.GetGalleryImagesToOpen()[i])
            {
                _warningIcon.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (_onePhotoActive)
        {
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    Vector2 auxPos = Input.GetTouch(0).position;

                    _positionFirstPinch = _mainCamera.ScreenToViewportPoint(auxPos);
                }
            }

            if (Input.touchCount == 2)
            {
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                {
                    Vector2 auxPos = Input.GetTouch(1).position;
                    _positionSecondPinch = _mainCamera.ScreenToViewportPoint(auxPos);
                    _pinchDistance = (_positionFirstPinch - _positionSecondPinch).magnitude;
                    _originalSizeDelta = _onePhotoMainImage.rectTransform.sizeDelta;
                    print(_originalSizeDelta);
                    //obtengo la posicion de la imagen
                    Vector2 _imgPosition = _onePhotoMainImage.rectTransform.position;
                    //obtengo la posición del punto central
                    Vector2 pointPosition = Input.GetTouch(0).position + ((Input.GetTouch(1).position - Input.GetTouch(0).position) / 2f);
                    //obtengo las dimensiones de la imagen
                    Vector2 imgDimensions = _onePhotoMainImage.rectTransform.sizeDelta;
                    Vector2 localPoint = Vector2.zero;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_onePhotoMainImage.rectTransform, pointPosition, null, out localPoint);
                    localPoint = new Vector2(localPoint.x / imgDimensions.x, localPoint.y / imgDimensions.y);

                    RectTransform _mainImageRectTransform = _onePhotoMainImage.rectTransform;
                    Vector2 prePosition = _mainImageRectTransform.position;
                    _mainImageRectTransform.pivot = localPoint;
                    _mainImageRectTransform.position = prePosition;

                    Vector2 finalLocalPoint = new Vector2(0.5f, 0.5f) + localPoint;
                    _mainImageRectTransform.pivot = finalLocalPoint;
                    _mainImageRectTransform.anchoredPosition = imgDimensions * localPoint;
                    _pinching = true;
                }
            }

            if (_pinching)
            {
                if (Input.touchCount < 2)
                {
                    _pinching = false;
                    _onePhotoMainImage.rectTransform.sizeDelta = _originalSizeDelta;
                    _onePhotoMainImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    _onePhotoMainImage.rectTransform.anchoredPosition = Vector3.zero;
                    _instanceCenter.SetZoomButtonState(true);
                    _previousButton.SetActive(true);
                    _nextButton.SetActive(true);
                }
                else
                {
                    float newPinchingDistance = (_mainCamera.ScreenToViewportPoint(Input.GetTouch(0).position) - _mainCamera.ScreenToViewportPoint(Input.GetTouch(1).position)).magnitude;
                    _instanceCenter.SetZoomButtonState(false);
                    _previousButton.SetActive(false);
                    _nextButton.SetActive(false);
                    if (newPinchingDistance > _pinchDistance)
                    {
                        float zoom = (1 + 3 * (newPinchingDistance - _pinchDistance));
                        _onePhotoMainImage.rectTransform.sizeDelta = _originalSizeDelta * zoom;
                    }
                }
            }
        }
    }
}
