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
    public static bool _canWorkDinosaur;

    private void Start()
    {
        UnlockEverything();
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
    public static void OnlyCanWork()
    {
        LockEverything();
        _canWorkDinosaur = true;
    }
    public static void UnlockEverything()
    {
        _canPurchase = true;
        _canPickDinosaur = true;
        _canDestroyDinosaur = true;
        _canMoveDinosaur = true;
        _canMergeDinosaur = true;
        _canWorkDinosaur = true;
    }
    public static void LockEverything()
    {
        _canPurchase = false;
        _canPickDinosaur = false;
        _canDestroyDinosaur = false;
        _canMoveDinosaur = false;
        _canMergeDinosaur = false;
        _canWorkDinosaur = false;
    }
}
