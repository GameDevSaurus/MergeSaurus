using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCardController : MonoBehaviour
{

    CardConfigurator _cardConfigurator;

    void Start()
    {
        _cardConfigurator = FindObjectOfType<CardConfigurator>();    
    }

    public void Refresh(int _cardIndex )
    {
        _cardConfigurator.Init(_cardIndex, true);
    }

    void Update()
    {
        
    }
}
