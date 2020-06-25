using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    public static bool _canPurchase;
    public static bool _canPickDinosaur;
    public static bool _canMoveDinosaur;
    public static bool _canMergeDinosaur;
    public static bool _canDestroyDinosaur;

    private void Start()
    {
        _canPurchase = true;
        _canPickDinosaur = true;
        _canDestroyDinosaur = true;
        _canMoveDinosaur = true;
        _canMergeDinosaur = true;
    }

    public static void OnlyCanPurchase()
    {
        _canPurchase = true;
        _canPickDinosaur = false;
        _canDestroyDinosaur = false;
        _canMoveDinosaur = false;
        _canMergeDinosaur = false;
    }
    public static void OnlyCanPick()
    {
        _canPurchase = false;
        _canPickDinosaur = true;
        _canDestroyDinosaur = false;
        _canMoveDinosaur = false;
        _canMergeDinosaur = false;
    }
    public static void OnlyCanMerge()
    {
        _canPurchase = false;
        _canPickDinosaur = true;
        _canDestroyDinosaur = false;
        _canMoveDinosaur = false;
        _canMergeDinosaur = true;
    }
    public static void UnlockEverything()
    {
        _canPurchase = true;
        _canPickDinosaur = true;
        _canDestroyDinosaur = true;
        _canMoveDinosaur = true;
        _canMergeDinosaur = true;
    }
    public static void LockEverything()
    {
        _canPurchase = false;
        _canPickDinosaur = false;
        _canDestroyDinosaur = false;
        _canMoveDinosaur = false;
        _canMergeDinosaur = false;
    }
}
