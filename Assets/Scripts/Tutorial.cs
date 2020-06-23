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

    bool waitingPurchase = false;
    bool waitingSpeak = false;

    private void Awake()
    {
        GameEvents.FastPurchase.AddListener(FastPurchase);
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
        _tutorController.Speak(1);
        waitingSpeak = true;
        while (waitingSpeak)
        {
            yield return null;
        }
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _fastPurchaseButtonTr.position;
        yield return StartCoroutine(ZoomIn());
        _handController.gameObject.SetActive(true);
        waitingPurchase = true;
        StartCoroutine(Tutorial1WaitForTouch());
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
        }
    }

    public void FastPurchase()
    {
        if (waitingPurchase)
        {
            waitingPurchase = false;
        }
    }
}
