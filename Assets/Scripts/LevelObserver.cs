using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LevelObserver : MonoBehaviour
{
    TextMeshProUGUI _levelText;
    [SerializeField]
    Image _expBar;
    private void Start()
    {
        _levelText = GetComponent<TextMeshProUGUI>();
        UpdateBar();
    }
    void Update()
    {
        if (UserDataController._checked)
        {
            _levelText.text = UserDataController.GetLevel().ToString();
            UpdateBar();
        }
    }

    public void UpdateBar()
    {
        _expBar.fillAmount = UserDataController.GetExperienceAmount();
    }
}