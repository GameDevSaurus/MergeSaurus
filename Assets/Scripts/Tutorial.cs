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
    HandController _handController;  //Referenciar sus Corutinas y detenerlas antes de desactivar el objeto
    List<Coroutine> _handCoroutines;
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    TutorController _tutorController;
    [SerializeField]
    AdviceController _adviceController;
    bool waitingPurchaseTutorial0 = false;
    bool waitingPurchaseTutorial1 = false;
    bool waitingSpeak = false;
    bool _canPurchase = false;
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
        _handCoroutines = new List<Coroutine>();
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

    IEnumerator Tutorial0()
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
        waitingPurchaseTutorial0 = true;
        StartCoroutine(TutorialWaitForTouch());
        yield return new WaitForSeconds(0.5f);
        _canPurchase = true;
    }

    IEnumerator Tutorial1()
    {
        _circlePanelObject.SetActive(true);
        _circlePanelTr.position = _fastPurchaseButtonTr.position;
        _handController.GetComponent<RectTransform>().position = _fastPurchaseButtonTr.position;
        yield return StartCoroutine(ZoomIn());
        _handController.gameObject.SetActive(true);
        waitingPurchaseTutorial1 = true;
        StartCoroutine(TutorialWaitForTouch());
        yield return new WaitForSeconds(0.5f);
        _canPurchase = true;
    }

    IEnumerator TutorialWaitForTouch()
    {
        if(_handCoroutines.Count == 0)
        {
            yield return StartCoroutine(_handController.CrAppear());
            //_handCoroutines.Add(_handController.CrAppear()));
            yield return StartCoroutine(_handController.CrPointIn());
            yield return StartCoroutine(_handController.CrDisappear());
        }

        _handController.ResetHand();
        if (waitingPurchaseTutorial0)
        {
            StartCoroutine(TutorialWaitForTouch());
        }
    }

    public void FastPurchase()
    {
        if (waitingPurchaseTutorial0)
        {
            waitingPurchaseTutorial0 = false;
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(0);
            _canPurchase = false;
            StartTutorial(1);
        }

        if (waitingPurchaseTutorial1)
        {
            waitingPurchaseTutorial0 = false;
            _handController.gameObject.SetActive(false);
            _circlePanelObject.SetActive(false);
            UserDataController.SaveTutorial(1);
            _canPurchase = false;
            //StartTutorial(2);
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

    public bool CanPurchase()
    {
        return _canPurchase;
    }
}
