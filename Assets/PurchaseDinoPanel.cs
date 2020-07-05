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
    Image _dinoImage;
    [SerializeField]
    Button _dinoButton;
    string purchaseCost;

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
        purchaseCost = cost;
    }

    public void SetDinoImage(Sprite image)
    {
        _dinoImage.sprite = image;
        _dinoImage.overrideSprite = image;
    }
    public void LockPanel()
    {
        _dinoImage.color = Color.black;
        _purchaseCost.text = "";
        _textName.enabled = false;
    }
    public void UnlockPanel()
    {
        _dinoImage.color = Color.white;
        _purchaseCost.text = purchaseCost;
        _textName.enabled = true;
    }
    public void LockPurcharse()
    {
        _dinoButton.interactable = false;
    }
    public void UnlockPurcharse()
    {
        _dinoButton.interactable = true;
    }
    public void SetDinoButtonMode(bool isActive)
    {
        _dinoButton.interactable = isActive;
    }

    public Button GetDinoButton()
    {
        return _dinoButton;
    }
}
