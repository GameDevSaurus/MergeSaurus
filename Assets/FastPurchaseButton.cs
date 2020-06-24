using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastPurchaseButton : MonoBehaviour
{
    [SerializeField]
    Button _fastPurchaseButton;

    public void Purchase()
    {
        if(UserDataController.GetEmptyCells() > 0)
        {
            MainGameSceneController.FastPurchase(0, 10);
        }
    }

    private void Update()
    {
        if(UserDataController.HaveMoney(10))
        {
            _fastPurchaseButton.interactable = true;
        }
        else
        {
            _fastPurchaseButton.interactable = false;
        }
    }
}
