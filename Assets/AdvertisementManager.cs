using System.Collections;
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
    string _speedUpPlacementID = "SpeedUp";
    [SerializeField]
    SpeedUpManager _speedUpManager;
    //ID DEL JUEGO --> 3701221
    void Start()
    {
        _speedUpButton.interactable = Advertisement.IsReady(_speedUpPlacementID);
        _speedUpButton.onClick.AddListener(ShowRewardedVideo);
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    void ShowRewardedVideo()
    {
        Advertisement.Show(_speedUpPlacementID);
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == _speedUpPlacementID)
        {
            _speedUpButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            _speedUpManager.SpeedUp();
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

    public void OnUnityAdsDidError(string errorMessage)
    {
        print(errorMessage);
    }
    public void OnUnityAdsDidStart(string placementId)
    {

    }

}
