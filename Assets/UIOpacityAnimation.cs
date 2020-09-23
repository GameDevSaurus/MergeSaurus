using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIOpacityAnimation : MonoBehaviour
{
    [SerializeField]
    AnimationCurve _animationCurve;
    Image _image;
    [SerializeField]
    float _time;
    float _elapsedTime;
    void Start()
    {
        _image = GetComponent<Image>();
    }

    void Update()
    {
        _elapsedTime +=Time.deltaTime;
        if (_elapsedTime > _time)
        {
            _elapsedTime = 0f;
            
        }
        _image.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, _animationCurve.Evaluate(_elapsedTime / _time));
    }
}
