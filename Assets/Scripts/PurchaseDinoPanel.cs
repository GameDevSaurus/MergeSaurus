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
    Sprite[] currencyImages;
    [SerializeField]
    Image _currencyImage;
    [SerializeField]
    Button _dinoButton;
    string purchaseCost;
    bool available = false;

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
        _currencyImage.sprite = currencyImages[0];
    }
    public void SetGemsCost(int cost)
    {
        _purchaseCost.text = cost.ToString();
        purchaseCost = cost.ToString();
        _currencyImage.sprite = currencyImages[1];
    }
    public void SetVideoButton()
    {
        _purchaseCost.text = "GRATIS";
        _currencyImage.sprite = currencyImages[2];
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
        available = false;
        _purchaseCost.text = "";
        _dinoButton.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f); //esto sera un cambio de sprite
    }
    public void UnlockPurcharse()
    {
        available = true;
        _purchaseCost.text = purchaseCost;
        _dinoButton.GetComponent<Image>().color = Color.green; //esto sera un cambio de sprite
    }
    public bool GetDinoButtonState()
    {
        return available;
    }

    public Button GetDinoButton()
    {
        return _dinoButton;
    }
}
