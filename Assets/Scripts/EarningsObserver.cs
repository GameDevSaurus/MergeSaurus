using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EarningsObserver : MonoBehaviour
{
    TextMeshProUGUI _earningText;
    EconomyManager _economyManager;
    private void Start()
    {
        _earningText = GetComponent<TextMeshProUGUI>();
        _economyManager = FindObjectOfType<EconomyManager>();
    }
    void Update()
    {
        GameCurrency gc = _economyManager.GetEarningsPerSecond();
        gc.MultiplyCurrency(0.5f);
        gc.MultiplyCurrency(CurrentSceneManager.GetGlobalSpeed());
        _earningText.text = gc.GetCurrentMoney() + "/sec";
    }
}
