using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastPurchaseButton : MonoBehaviour
{
    [SerializeField]
    Button _fastPurchaseButton;
    [SerializeField]
    MainGameSceneController _mainGameSceneController;
    public void Purchase()
    {
        if(UserDataController.GetEmptyCells() > 0)
        {
            _mainGameSceneController.FastPurchase(0, 10);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke("ADVICE_NOEMPTYCELLS");
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
