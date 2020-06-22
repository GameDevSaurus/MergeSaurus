using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoftCoinsObserver : MonoBehaviour
{
    TextMeshProUGUI _softCoinText;
    private void Start()
    {
        _softCoinText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (UserDataController._checked)
        {
            _softCoinText.text = UserDataController._currentUserData._softCoins.ToString();
        }
    }
}
