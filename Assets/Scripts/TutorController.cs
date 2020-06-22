using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorController : MonoBehaviour
{
    public RectTransform _rectTrTutor, _rectTrFade, _rectTrTxBubble;

    public List<List<string>> _conversations;

    public TextMeshProUGUI _textConversation;

    Image fadeImage;
    Color transparentWhite;

    public AnimationCurve _tutorCurve, _bounceCurve;
    private void Start()
    {
        _conversations = new List<List<string>>();
        List<string> introConversation = new List<string>();
        introConversation.Add("CONV_0_0");
        introConversation.Add("CONV_0_1");
        _conversations.Add(introConversation);
        fadeImage = _rectTrFade.GetComponent<Image>();
        transparentWhite = new Color(1, 1, 1, 0);
        fadeImage.color = transparentWhite;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Speak();
        }
    }
    public void Speak()
    {
        StartCoroutine(CrSpeakIn());
    }

    IEnumerator CrSpeakIn()
    {
        float appearAnimationTime = 0.5f;
        float bouncedAnimationTime = 0.25f;
        Vector3 initialPos = _rectTrTutor.anchoredPosition;
        for (float i = 0; i< appearAnimationTime; i+= Time.deltaTime)
        {
            _rectTrTutor.anchoredPosition = Vector2.Lerp(initialPos, Vector2.zero, _tutorCurve.Evaluate(i / appearAnimationTime));
            fadeImage.color = Color.Lerp(transparentWhite, Color.white, i / appearAnimationTime);
            yield return null;
        }
        _rectTrTxBubble.GetComponent<Image>().color = Color.white;
        fadeImage.color = Color.white;
        _rectTrTxBubble.localScale = Vector2.zero;
        _rectTrTutor.anchoredPosition = Vector2.zero;
        for (float i = 0; i < bouncedAnimationTime; i += Time.deltaTime)
        {
            _rectTrTutor.localScale = new Vector3(1,1,1) + new Vector3(1, 1, 1)*(_bounceCurve.Evaluate(i / bouncedAnimationTime)*0.1f);
            _rectTrTxBubble.GetComponent<Image>().color = Color.Lerp(transparentWhite, Color.white, i / bouncedAnimationTime);
            _rectTrTxBubble.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, i/bouncedAnimationTime);
            yield return null;
        }

        //StartCoroutine(Grow());
    }
    IEnumerator Grow()
    {
        Vector2 initialDeltaSize = _rectTrTutor.sizeDelta;
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            _rectTrTutor.sizeDelta = initialDeltaSize + new Vector2(100 * i, 100 * i);
            yield return null;
        }
        _rectTrTutor.sizeDelta = initialDeltaSize + new Vector2(100, 100);
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            _rectTrTutor.sizeDelta = initialDeltaSize - new Vector2(100 * i, 100 * i);
            yield return null;
        }
        _rectTrTutor.sizeDelta = initialDeltaSize;
    }
}
