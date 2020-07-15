﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdvertisementManager : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
        private string gameId = "";
#elif UNITY_ANDROID
    private string gameId = "3701221";
#endif
    [SerializeField]
    Button _speedUpButton;
    string _placementID;
    SpeedUpManager _speedUpManager;
    SpinManager _spinManager;
    PassiveGainManager _passiveGainManager;
    //ID DEL JUEGO --> 3701221
    BoxManager _boxManager;
    RewardManager _rewardManager;
    bool justWatchedAd = false;

    private void Awake()
    {
        GameEvents.PlayAd.AddListener(PlayVideo);
        _speedUpManager = FindObjectOfType<SpeedUpManager>();
        _boxManager = FindObjectOfType<BoxManager>();
        _spinManager = FindObjectOfType<SpinManager>();
        _passiveGainManager = FindObjectOfType<PassiveGainManager>();
        _rewardManager = FindObjectOfType<RewardManager>();
    }
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    public void PlayVideo(string placementID)
    {
        _placementID = placementID;
        ShowRewardedVideo();
    }
    void ShowRewardedVideo()
    {
        Advertisement.Show(_placementID);
    }

    public void OnUnityAdsReady(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            justWatchedAd = true;
            switch (placementId)
            {
                case "SpeedUp":
                    _rewardManager.EarnSpeedUp(200);
                    break;

                case "SpecialBox":
                    _boxManager.RewardBox(4); 
                    break;
                case "SpinReward":
                    _spinManager.SpinCallBack();
                    break;
                case "PassiveEarnings":
                    _passiveGainManager.VideoWatchedCallBack();
                    break;
            }
        }
        else
        {
            if (showResult == ShowResult.Skipped)
            {
                print("Cancelaron el video");
            }
            else
            {
                if (showResult == ShowResult.Failed)
                {
                    print("El video falló :____(");
                }
            }
        }
    }

    private void Update()
    {
        if (justWatchedAd)
        {
            justWatchedAd = false;
            UserDataController.AddDailyAd();
        }
    }
    public void OnUnityAdsDidError(string errorMessage)
    {
        print(errorMessage);
    }
    public void OnUnityAdsDidStart(string placementId)
    {

    }

}