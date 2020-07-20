using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardInstance : MonoBehaviour
{
    [SerializeField]
    Image background, upperBorder, lowerborder, button, buttonBorder;
    [SerializeField]
    Color bgC1, bgC2, bordersC1, bordersC2, buttonC1, buttonC2, buttonBorderC1, buttonBorderC2;
    public void BasicConfig()
    {
        background.color = bgC1;
        upperBorder.color = bordersC1;
        lowerborder.color = bordersC1;
        button.color = buttonC1;
        buttonBorder.color = buttonBorderC1;
    }
    public void SelectedConfig()
    {
        background.color = bgC2;
        upperBorder.color = bordersC2;
        lowerborder.color = bordersC2;
        button.color = buttonC2;
        buttonBorder.color = buttonBorderC2;
    }
}
