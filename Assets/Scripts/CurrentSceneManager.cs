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
    public static bool _canShowDinosaurByTouch;
    public static bool _canShowDinosaurByDrag;
    public static bool _canOpenBox;
    public static bool _canTakeBackByCell;
    public static bool _canTakeBackByExpositor;
    public static float _currentGlobalSpeed;
    public static bool _canOpenShop;
    public static bool _canChangeName;
    private void Start()
    {
        _currentGlobalSpeed = 1f;
        UnlockEverything();
    }

    public static void SetGlobalSpeed(int globalSpeed)
    {
        _currentGlobalSpeed = globalSpeed;
    }
    public static float GetGlobalSpeed()
    {
        return _currentGlobalSpeed * (1 + (UpgradesManager.GetExtraTouristSpeed()/100f));
    }
    public static void OnlyCanPurchase()
    {
        LockEverything();
        _canPurchase = true;
    }
    public static void OnlyCanPick()
    {
        LockEverything();
        _canPickDinosaur = true;
    }
    public static void OnlyCanMerge()
    {
        LockEverything();
        _canPickDinosaur = true;
        _canMergeDinosaur = true;
    }
    public static void OnlyCanShowByDrag()
    {
        LockEverything();
        _canPickDinosaur = true;
        _canShowDinosaurByDrag = true;
    }
    public static void OnlyCanShowByTouch()
    {
        LockEverything();
        _canShowDinosaurByTouch = true;
    }
    public static void OnlyCanOpenBox()
    {
        LockEverything();
        _canOpenBox = true;
    }
    public static void OnlyCanTakeBackByCell()
    {
        LockEverything();
        _canTakeBackByCell = true;
    }
    public static void OnlyCanTakeBackByExpositor()
    {
        LockEverything();
        _canTakeBackByExpositor = true;
    }
    public static void UnlockEverything()
    {
        _canPurchase = true;
        _canPickDinosaur = true;
        _canDestroyDinosaur = true;
        _canMoveDinosaur = true;
        _canMergeDinosaur = true;
        _canShowDinosaurByDrag = true;
        _canShowDinosaurByTouch = true;
        _canOpenBox = true;
        _canTakeBackByCell = true;
        _canTakeBackByExpositor = true;
        _canChangeName = true;
        _canOpenShop = true;
    }
    public static void LockEverything()
    {
        _canPurchase = false;
        _canPickDinosaur = false;
        _canDestroyDinosaur = false;
        _canMoveDinosaur = false;
        _canMergeDinosaur = false;
        _canShowDinosaurByDrag = false;
        _canShowDinosaurByTouch = false;
        _canOpenBox = false;
        _canTakeBackByCell = false;
        _canTakeBackByExpositor = false;
        _canOpenShop = false;
        _canChangeName = false;
    }
}
