using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationSceneController : MonoBehaviour
{
    public float barFillDuration = 2f;
    float _loadAmount;
    // Start is called before the first frame update
    void Start()
    {
        if (!UserDataController.Exist())
        {
            UserDataController.InitializeUser();
            StartCoroutine(LoadingBar());
        }
        else
        {
            UserDataController.LoadFromFile();
            StartCoroutine(LoadingBar());
        }   
    }

    public IEnumerator LoadingBar()
    {
        for(float i = 0; i < barFillDuration; i += Time.deltaTime)
        {
            yield return null;
           _loadAmount = i / barFillDuration;
        }
        _loadAmount = 1f;
        GameEvents.LoadScene.Invoke("MainGame");
    }

    public float GetLoadAmount()
    {
        return _loadAmount;
    }
}
