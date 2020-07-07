using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class SpeedUpRewardButton : MonoBehaviour, IUnityAdsListener
{

    #if UNITY_IOS
        private string gameId = "";
    #elif UNITY_ANDROID
        private string gameId = "3701221";
#endif

    Button _button;
    string myPlacementID = "SpeedUp";
    [SerializeField]
    SpeedUpManager _speedUpManager;
    //ID DEL JUEGO --> 3701221
    void Start()
    {
        _button = GetComponent<Button>();
        _button.interactable = Advertisement.IsReady(myPlacementID);
        _button.onClick.AddListener(ShowRewardedVideo);
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementID);
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementID)
        {
            _button.interactable = true;
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
            if(showResult == ShowResult.Skipped)
            {
                print("Cancelaron el video");
            }
            else
            {
                if(showResult == ShowResult.Failed)
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
