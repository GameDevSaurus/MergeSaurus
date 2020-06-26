using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    float zoomDuration = 0.55f;
    [SerializeField]
    RectTransform _circlePanelTr;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    RectTransform _fastPurchaseButtonTr;
    [SerializeField]
    GameObject _circlePanelObject;
    Camera _camera;
    [SerializeField]
    HandController _handController;
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    TutorController _tutorController;
    [SerializeField]
    AdviceController _adviceController;
    bool waitingPurchaseTutorial0 = false;
    bool waitingPurchaseTutorial1 = false;
    bool waitingMergeTutorial2 = false;
    bool waitingWorkDinosaurTutorial3 = false;
    bool waitingSpeak = false;
    Coroutine _adviceCr;
    Coroutine _showTextCr;
    Coroutine _moveTextCr;
    Coroutine _hideTextCr;

    private void Awake()
    {
        GameEvents.FastPurchase.AddListener(FastPurchase);
        GameEvents.ShowAdvice.AddListener(ShowAdvice);
        GameEvents.MergeDino.AddListener(Merge);
        GameEvents.WorkDino.AddListener(Work);
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
    }
    private void Start()
    {
        _camera = Camera.main;
        if (!UserDataController._currentUserData._tutorialCompleted[0])
        {
            StartTutorial(0);
        }
        else
        {
            if (!UserDataController._currentUserData._tutorialCompleted[1])
            {
                StartTutorial(1);
            }
            else
            {
                if (!UserDataController._currentUserData._tutorialCompleted[2])
                {
                    StartTutorial(2);
                }
                else
                {
                    if (!UserDataController._currentUserData._tutorialCompleted[3])
                    {
                        StartTutorial(3);
                    }
                }
            }
        }
    }

    public void StartTutorial(int tutorialIndex)
    {
        switch (tutorialIndex)
        {
            case 0:
                StartCoroutine(Tutorial0());
                break;
            case 1:
                StartCoroutine(Tutorial1());
                break;
            case 2:
                StartCoroutine(Tutorial2());
                break;
            case 3:
                StartCoroutine(Tutorial3());
                break;
        }
    }

    public void EndSpeak()
    {
        waitingSpeak = false;
    }

    IEnumerator ZoomIn(float targetScale)
    {
        for(float i = 0; i< zoomDuration; i+= Time.deltaTime)
        {
            _circlePanelTr.localScale = Vector3.Lerp(Vector3.one * 25, Vector3.one * targetScale, _animationCurve.Evaluate(i / zoomDuration));
            yield return null;
        }
        _circlePanelTr.localScale = Vector3.one * targetScale;
    }

    IEnumerator ZoomOut(float originalScale)
    {
        for (float i = 0; i < zoomDuration; i += Time.deltaTime)
        {
            _circlePanelTr.localScale = Vector3.Lerp(Vector3.one * originalScale, Vector3.one * 25, _animationCurve.Evaluate(i / zoomDuration));
            yield return null;
        }
        _circlePanelTr.localScale = Vector3.one * 25f;
        _circlePanelObject.SetActive(false);
    }

    IEnumerator Tutorial0()
    {
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(0);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }

        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _fastPurchaseButtonTr.position;
        _handController.GetComponent<RectTransform>().position = _fastPurchaseButtonTr.position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingPurchaseTutorial0 = true;
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanPurchase();
    }

    IEnumerator Tutorial1()
    {
        CurrentSceneManager.LockEverything();
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _fastPurchaseButtonTr.position;
        _handController.GetComponent<RectTransform>().position = _fastPurchaseButtonTr.position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingPurchaseTutorial1 = true;
        _handController.StartTouchMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanPurchase();
    }
    IEnumerator Tutorial2()
    {
        Vector3 dino1PositionUI = _mainGameSceneController.GetDinoPositionsUIByCell(0);
        Vector3 dino2PositionUI = _mainGameSceneController.GetDinoPositionsUIByCell(1);
        Vector3 middlePosition = dino1PositionUI + ((dino2PositionUI - dino1PositionUI) / 2f);
        CurrentSceneManager.LockEverything();
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = middlePosition;
        yield return StartCoroutine(ZoomIn(1.75f));
        _handController.gameObject.SetActive(true);
        waitingMergeTutorial2 = true;
        _handController.StartDragMode(dino1PositionUI, dino2PositionUI);
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanMerge();
    }
    IEnumerator Tutorial3()
    {
        _tutorController.gameObject.SetActive(true);
        _tutorController.Speak(1);
        CurrentSceneManager.LockEverything();
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _camera.WorldToScreenPoint(_mainGameSceneController.GetFirstDinoPosition());
        _handController.GetComponent<RectTransform>().position = _circlePanelTr.position;
        yield return StartCoroutine(ZoomIn(1f));
        _handController.gameObject.SetActive(true);
        waitingWorkDinosaurTutorial3 = true;
        _handController.StartDubleClickMode();
        yield return new WaitForSeconds(0.5f);
        CurrentSceneManager.OnlyCanWork();
    }
    #region EventsCallbacks
    public void FastPurchase()
    {
        if (waitingPurchaseTutorial0)
        {
            waitingPurchaseTutorial0 = false;
            _handController.StopTouchCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(0);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(1);
        }

        if (waitingPurchaseTutorial1)
        {
            waitingPurchaseTutorial1 = false;
            _handController.StopTouchCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(1);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(2);
        }
    }
    public void Merge()
    {
        if (waitingMergeTutorial2)
        {
            waitingMergeTutorial2 = false;
            _handController.StopDragCoroutines();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(2);
            CurrentSceneManager.UnlockEverything();
            StartTutorial(3);
        }
    }
    public void Work()
    {
        if (waitingWorkDinosaurTutorial3)
        {
            waitingWorkDinosaurTutorial3 = false;
            _handController.StopDoubleClick();
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(3);
            CurrentSceneManager.UnlockEverything();
            //StartTutorial(4);
        }
    }

    #endregion
    public void ShowAdvice(string adviceKey)
    {
        if (_adviceCr != null)
        {
            StopCoroutine(_adviceCr);

            if (_showTextCr != null)
            {
                StopCoroutine(_showTextCr);
            }
            if (_hideTextCr != null)
            {
                StopCoroutine(_hideTextCr);
            }
            if (_moveTextCr != null)
            {
                StopCoroutine(_moveTextCr);
            }
        }
        _adviceCr = StartCoroutine(CrShowAdvice(adviceKey));
    }

    IEnumerator CrShowAdvice(string adviceKey)
    {
        _adviceController.gameObject.SetActive(true);
        _moveTextCr = StartCoroutine(_adviceController.CrMoveText());
        yield return _showTextCr = StartCoroutine(_adviceController.CrShowText(adviceKey));
        yield return new WaitForSeconds(_adviceController.moveDuration - (_adviceController.fadeDuration *2f));
        yield return _hideTextCr = StartCoroutine(_adviceController.CrHideText());
        _adviceController.gameObject.SetActive(false);
    }
}
