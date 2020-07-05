using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DinoProgressObserver : MonoBehaviour
{
    Image _progressBar;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;
    [SerializeField]
    TextMeshProUGUI txProgress;
    [SerializeField]
    TextMeshProUGUI currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        _progressBar = GetComponent<Image>();
        UpdateFillAmount();
    }

    public void UpdateFillAmount()
    {
        float currentProgress = _mainGameSceneController.GetDinosSum();
        float targetProgress = Mathf.Pow(2,UserDataController.GetBiggestDino()+1);
        float finalAmount = currentProgress / targetProgress;
        currentLevel.text = (UserDataController.GetBiggestDino() + 2).ToString();
        _progressBar.fillAmount = finalAmount;
        txProgress.text = Mathf.Floor(finalAmount * 100).ToString() + "%";
    }

    private void Update()
    {
        UpdateFillAmount();
    }
}
