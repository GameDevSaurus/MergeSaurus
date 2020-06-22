using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationSceneController : MonoBehaviour
{
    public TimeController tController;

    // Start is called before the first frame update
    void Start()
    {
        if (!UserDataController.Exist())
        {
            UserDataController.InitializeUser(tController.GetTimeNow().ToBinary().ToString());
            GameEvents.LoadScene.Invoke("Main");
        }
        else
        {
            UserDataController.LoadFromFile();
            GameEvents.LoadScene.Invoke("Main");
        }
    }
}
