using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelObserver : MonoBehaviour
{
    TextMeshProUGUI _levelText;
    private void Start()
    {
        _levelText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (UserDataController._checked)
        {
            _levelText.text = UserDataController.GetLevel().ToString();
        }
    }
}