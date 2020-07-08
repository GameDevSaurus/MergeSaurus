using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationSceneController : MonoBehaviour
{
    public float barFillDuration = 2f;
    float _loadAmount;
    bool _waitForTime = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!UserDataController.Exist())
        {
            _waitForTime = true;
        }
        else
        {
            UserDataController.LoadFromFile();
            StartCoroutine(LoadingBar());
        }   
    }

    public void Update()
    {
        if (_waitForTime)
        {
            if (TimeController._timeChecked)
            {
                UserDataController.InitializeUser(TimeController.GetTimeNow().ToBinary().ToString());
                _waitForTime = false;
                StartCoroutine(LoadingBar());
            }
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
