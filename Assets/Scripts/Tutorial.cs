using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    float zoomDuration = 0.55f;
    [SerializeField]
    RectTransform _circlePanelTr;

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
    bool waitingPurchase = false;
    bool waitingSpeak = false;
    Coroutine _adviceCr;
    Coroutine _showTextCr;
    Coroutine _moveTextCr;
    Coroutine _hideTextCr;

    private void Awake()
    {
        GameEvents.FastPurchase.AddListener(FastPurchase);
        GameEvents.ShowAdvice.AddListener(ShowAdvice);
    }
    private void Start()
    {
        _camera = Camera.main;
        StartTutorial(0);
    }

    public void StartTutorial(int tutorialIndex)
    {
        switch (tutorialIndex)
        {
            case 0:
                StartCoroutine(Tutorial1());
                break;
        }
    }

    public void EndSpeak()
    {
        waitingSpeak = false;
    }

    IEnumerator ZoomIn()
    {
        for(float i = 0; i< zoomDuration; i+= Time.deltaTime)
        {
            _circlePanelTr.localScale = Vector3.Lerp(Vector3.one * 25, Vector3.one, _animationCurve.Evaluate(i / zoomDuration));
            yield return null;
        }
        _circlePanelTr.localScale = Vector3.one;
    }
    IEnumerator ZoomOut()
    {
        for (float i = 0; i < zoomDuration; i += Time.deltaTime)
        {
            _circlePanelTr.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 25, _animationCurve.Evaluate(i / zoomDuration));
            yield return null;
        }
        _circlePanelTr.localScale = Vector3.one * 25f;
        _circlePanelObject.SetActive(false);
    }

    IEnumerator Tutorial1()
    {
        if (!UserDataController._currentUserData._tutorialCompleted[0])
        {
            _tutorController.gameObject.SetActive(true);
            _tutorController.Speak(0);
            waitingSpeak = true;
            while (waitingSpeak)
            {
                yield return null;
            }
            _circlePanelObject.SetActive(true);
            _circlePanelTr.position = _fastPurchaseButtonTr.position;
            _handController.GetComponent<RectTransform>().position = _fastPurchaseButtonTr.position; 
            yield return StartCoroutine(ZoomIn());
            _handController.gameObject.SetActive(true);
            waitingPurchase = true;
            StartCoroutine(Tutorial1WaitForTouch());
        }
        else
        {
            if (UserDataController._currentUserData._tutorialCompleted[1])
            {
                //Tutorial 2
            }
        }
    }

    IEnumerator Tutorial1WaitForTouch()
    {
        yield return StartCoroutine(_handController.CrAppear());
        yield return StartCoroutine(_handController.CrPointIn());
        yield return StartCoroutine(_handController.CrDisappear());
        _handController.ResetHand();
        if (waitingPurchase)
        {
            StartCoroutine(Tutorial1WaitForTouch());
        }
        else
        {
            StartCoroutine(_handController.CrPointOut());
            StartCoroutine(ZoomOut());
            UserDataController.SaveTutorial(0);
        }
    }

    public void FastPurchase()
    {
        if (waitingPurchase)
        {
            waitingPurchase = false;
        }
    }

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
