using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    RectTransform _selfTransform;
    [SerializeField]
    RectTransform _pivotRotation;
    [SerializeField]
    Image _handImage;
    [SerializeField]
    AnimationCurve _animationCurve;
    [SerializeField]
    Image _touchCircleImage;

    float animDuration = 0.25f;
    float pointDuration = 0.5f;
    Color transparentWhite = new Color(1,1,1,0);
    // Start is called before the first frame update
    void Start()
    {
        _selfTransform = GetComponent<RectTransform>();
    }

    public void SetPosition(Vector3 targetPosition)
    {
        _selfTransform.position = targetPosition;
    }

    public void Appear()
    {
        StartCoroutine(CrAppear());
    }

    public void ResetHand()
    {
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, -40));
    }

    public IEnumerator CrAppear()
    {
        _handImage.gameObject.SetActive(true);
        for (float i = 0; i< animDuration; i+=Time.deltaTime)
        {
            _handImage.color = Color.Lerp(transparentWhite, Color.white, _animationCurve.Evaluate(i / animDuration));
            yield return null;
        }
        _handImage.color = Color.white;
    }

    public IEnumerator CrDisappear()
    {
        for (float i = 0; i < animDuration; i += Time.deltaTime)
        {
            _handImage.color = Color.Lerp(Color.white, transparentWhite, _animationCurve.Evaluate(i / animDuration));
            yield return null;
        }
        _handImage.color = transparentWhite;
        _handImage.gameObject.SetActive(false);
    }

    public IEnumerator CrPointIn()
    {
        for (float i = 0; i < pointDuration; i += Time.deltaTime)
        {
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0 + (40 * _animationCurve.Evaluate(i / pointDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        for (float i = 0; i < pointDuration; i += Time.deltaTime)
        {
            _touchCircleImage.rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _animationCurve.Evaluate( i / pointDuration));
            _touchCircleImage.color = Color.Lerp(Color.white, transparentWhite, _animationCurve.Evaluate( i / pointDuration));
            yield return null;
        }
        _touchCircleImage.rectTransform.localScale = Vector3.one;
        _touchCircleImage.color = transparentWhite;
    }
    public IEnumerator CrPointOut()
    {
        for (float i = 0; i < pointDuration; i += Time.deltaTime)
        {
            _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0 - (40 * _animationCurve.Evaluate(i / pointDuration))));
            yield return null;
        }
        _pivotRotation.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
