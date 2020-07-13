using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinAdvice : MonoBehaviour
{
    TextMeshProUGUI _adviceText;
    [SerializeField]
    AnimationCurve _animCurve;

    float moveDuration = 1f;

    float fadeDuration = 0.25f;
    Color transParentWhite = new Color(1, 1, 1, 0);

    private void Awake()
    {
        _adviceText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Play(Vector3 targetPos, GameCurrency earnedCoins)
    {
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(targetPos);
        StartCoroutine(AdviceManage(earnedCoins));
    }

    IEnumerator AdviceManage(GameCurrency nCoins)
    {
        _adviceText.rectTransform.anchoredPosition = new Vector2(0, 50);
        yield return  StartCoroutine(CrShowText(nCoins.GetCurrentMoneyConvertedTo3Chars()));
        StartCoroutine(CrMoveText());
        yield return new WaitForSeconds(moveDuration - fadeDuration * 2f);
        yield return StartCoroutine(CrHideText());
    }

    public IEnumerator CrShowText(string earnedCoins)
    {
        _adviceText.text = earnedCoins;
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            _adviceText.color = Color.Lerp(transParentWhite, Color.white, _animCurve.Evaluate(i / fadeDuration));
            yield return null;
        }
        _adviceText.color = Color.white;
    }
    public IEnumerator CrMoveText()
    {
        for (float i = 0; i < moveDuration; i += Time.deltaTime)
        {
            _adviceText.rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(0, 50), new Vector2(0, 100), _animCurve.Evaluate(i / moveDuration));
            yield return null;
        }
        _adviceText.rectTransform.anchoredPosition = new Vector2(0, 100);
    }
    public IEnumerator CrHideText()
    {
        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            _adviceText.color = Color.Lerp(Color.white, transParentWhite, _animCurve.Evaluate(i / fadeDuration));
            yield return null;
        }
        _adviceText.color = transParentWhite;
    }
}
