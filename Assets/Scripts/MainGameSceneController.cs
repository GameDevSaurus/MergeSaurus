using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameSceneController : MonoBehaviour
{
    public static void FastPurchase(int dinosaurIndex, int cost)
    {

        GameEvents.FastPurchase.Invoke();
    }
}
