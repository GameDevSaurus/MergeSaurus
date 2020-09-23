using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardInstance : MonoBehaviour
{
    [SerializeField]
    Image background, button, reward;
    [SerializeField]
    Color bgC1, bgC2, buttonCLocked;
    [SerializeField]
    Sprite yellowB, grayB;

    public void UsedConfig()
    {
        print("Inuse");
        background.color = bgC2;
        button.sprite = grayB;
        button.overrideSprite = grayB;
        button.color = Color.white;
        reward.color = Color.gray;
    }
    public void SelectedConfig()
    {
        background.color = bgC2;
        button.sprite = yellowB;
        button.overrideSprite = yellowB;
        button.color = Color.white;
        reward.color = Color.white;
    }
    public void BasicConfig()
    {
        background.color = bgC1;
        button.sprite = yellowB;
        button.overrideSprite = yellowB;
        button.color = buttonCLocked;
        reward.color = Color.gray;
    }
}
