using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLocalizator : MonoBehaviour
{
    Image splashArtImage;
    [SerializeField]
    string path;
    void Start()
    {
        splashArtImage = GetComponent<Image>();
        string finalPath = Application.productName + path;
        splashArtImage.sprite = Resources.Load<Sprite>(finalPath);
    }
}
