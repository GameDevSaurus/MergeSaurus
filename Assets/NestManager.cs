using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mainPanel;
    [SerializeField]
    AnimationCurve animationCurve;
    [SerializeField]
    BoxManager _boxManager;
    [SerializeField]
    EconomyManager _economyManager;
    public void OpenNest()
    {
        StartCoroutine(CrOpen());
    }
    public void CloseNest()
    {
        _mainPanel.SetActive(false);
    }
    public void ShowVideo()
    {
        GameEvents.PlayAd.Invoke("SpecialBox");
        CloseNest();
    }
    IEnumerator CrOpen()
    {
        RectTransform rt = _mainPanel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        _mainPanel.SetActive(true);
        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(i / 0.25f));
            yield return null;
        }
        rt.localScale = Vector3.one;
    }
    public void HardCoinPurchase()
    {
        if (_economyManager.SpendHardCoins(3))
        {
            _boxManager.RewardBox(4);
        }
        CloseNest();
    }
}