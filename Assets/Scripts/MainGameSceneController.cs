using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneController : MonoBehaviour
{
    public void FastPurchase()
    {
        GameEvents.FastPurchase.Invoke();
    }
}
