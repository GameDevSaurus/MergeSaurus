﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MergeUpManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _dinoTypeTx;
    [SerializeField]
    GameObject _mergeUpPanel;
    bool canDisable;
    [SerializeField]
    Image lastQualityBar;
    [SerializeField]
    Image currentQualityBar;
    Tutorial _tutorial;
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    Image dinoImage;
    [SerializeField]
    Sprite[] dinoMergeUpSprites;
    [SerializeField]
    AnimationCurve animationCurve;

    private void Awake()
    {
        _mergeUpPanel.SetActive(false);
        GameEvents.DinoUp.AddListener(MergeUpCallBack);
        _tutorial = FindObjectOfType<Tutorial>();
        _mainGameSceneController = FindObjectOfType<MainGameSceneController>();
    }
    public void MergeUpCallBack(int dinoType)
    {
        StartCoroutine(ShowNewMergeInfo(dinoType + 1));
    }

    IEnumerator ShowNewMergeInfo(int dinoType)
    {
        RectTransform rt = _mergeUpPanel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;
        _mergeUpPanel.SetActive(true);

        dinoImage.sprite = dinoMergeUpSprites[dinoType - 1];
        canDisable = false;
        _dinoTypeTx.text = DayCareManager.dinoNames[dinoType - 1];
        currentQualityBar.fillAmount = (float)(dinoType - 1f) / (float)UserDataController._currentUserData._dinosaurs.Length;
        lastQualityBar.fillAmount = (float)dinoType / (float)UserDataController._currentUserData._dinosaurs.Length;

        for (float i = 0; i < 0.25f; i += Time.deltaTime)
        {
            rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, animationCurve.Evaluate(i / 0.25f));
            yield return null;
        }
        rt.localScale = Vector3.one;  
        yield return new WaitForSeconds(0.5f);
        canDisable = true;
    }

    public void DisableMainPanel()
    {
        if (canDisable)
        {
            _mainGameSceneController.StopWaitingAnim();
            _mergeUpPanel.SetActive(false);
        }
    }
}
