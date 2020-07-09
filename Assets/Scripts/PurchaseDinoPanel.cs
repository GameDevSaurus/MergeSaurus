using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PurchaseDinoPanel : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _textName;
    [SerializeField]
    TextMeshProUGUI _textProfitsPerSec;
    [SerializeField]
    TextMeshProUGUI _purchaseCost;
    [SerializeField]
    TextMeshProUGUI _gemsCost;
    [SerializeField]
    Image _dinoImage;
    [SerializeField]
    Sprite[] currencyImages;
    [SerializeField]
    Image _currencyImage;
    [SerializeField]
    Button _dinoButton;
    [SerializeField]
    Button _videoButton;
    [SerializeField]
    Button _lockButton;
    [SerializeField]
    Button _gemButton;
    int _gemCost;

    public void SetDinoName(string n)
    {
        _textName.text = n;
    }

    public void SetProfits(string profits)
    {
        _textProfitsPerSec.text = profits + "/sec";
    }

    public void SetPurchaseCost(string cost)
    {
        _purchaseCost.text = cost;
    }
    public void SetGemsCost(int cost)
    {
        _gemsCost.text = cost.ToString();
        _gemCost = cost;
    }

    public void SetDinoImage(Sprite image)
    {
        _dinoImage.sprite = image;
        _dinoImage.overrideSprite = image;
    }
    public void LockPanel()
    {
        _dinoImage.color = Color.black;
        _textName.enabled = false;
        ShowLockButton();
    }
    public void UnlockPanel(int unlockType)
    {
        if(unlockType == 0)
        {
            ShowDefaultButton();
        }
        if (unlockType == 1)
        {
            ShowGemButton();
        }
        if (unlockType == 2)
        {
            ShowVideoButton();
        }
        _dinoImage.color = Color.white;
        _textName.enabled = true;
    }
    public void LockPurcharse()
    {
        ShowLockButton();
    }

    public Button GetDinoButton()
    {
        return _dinoButton;
    }
    public Button GetGemsButton()
    {
        return _gemButton;
    }
    public Button GetVideoButton()
    {
        return _videoButton;
    }

    public int GetGemCost()
    {
        return _gemCost;
    }

    public void SetPurchaseState(bool haveMoney)
    {
        _dinoButton.interactable = haveMoney;
    }
    public void ShowDefaultButton()
    {
        _dinoButton.gameObject.SetActive(true);
        _lockButton.gameObject.SetActive(false);
        _videoButton.gameObject.SetActive(false);
        _gemButton.gameObject.SetActive(false);
    }
    public void ShowLockButton()
    {
        _lockButton.gameObject.SetActive(true);
        _dinoButton.gameObject.SetActive(false);
        _videoButton.gameObject.SetActive(false);
        _gemButton.gameObject.SetActive(false);
    }
    public void ShowGemButton()
    {
        _gemButton.gameObject.SetActive(true);
        _lockButton.gameObject.SetActive(false);
        _dinoButton.gameObject.SetActive(false);
        _videoButton.gameObject.SetActive(false);
    }
    public void ShowVideoButton()
    {
        _videoButton.gameObject.SetActive(true);
        _lockButton.gameObject.SetActive(false);
        _dinoButton.gameObject.SetActive(false);
        _gemButton.gameObject.SetActive(false);
    }
}
