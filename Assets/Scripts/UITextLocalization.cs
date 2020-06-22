using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextLocalization : MonoBehaviour
{
    [SerializeField]
    string key;
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = LocalizationController._localizedData[key];
    }

    void Update()
    {
        
    }
}
