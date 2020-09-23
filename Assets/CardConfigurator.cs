using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardConfigurator : MonoBehaviour
{

    [SerializeField]
    Image _mainImage;
    [SerializeField]
    Image _frame;
    [SerializeField]
    Image _background;
    [SerializeField]
    Material _amazingBackgroundMaterial;
    [SerializeField]
    Image _foreground;
    [SerializeField]
    Material _amazingMaterial;
    [SerializeField]
    GameObject[] _vfx;
    
    static int[] _effects0 = {0,1,2};
    static List<Color> _colors;


    private void Start()
    {
        if(_colors != null)
        {
            _colors = new List<Color>();
            _colors.Add(new Color(0.65f, 0.21f, 0.144f));
            _colors.Add(new Color( 0.145f,0.2012f,0.6509f));
        }
    }
    public void Init(int character)
    {
        _mainImage.sprite = Resources.Load<Sprite>("Sprites/BigDraws/" + character);
        _mainImage.overrideSprite = Resources.Load<Sprite>("Sprites/BigDraws/" + character);

        _frame.sprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        _frame.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());

        _background.sprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (character / 4));
        _background.overrideSprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (character / 4));
        _background.preserveAspect = false;

        if (UserDataController.IsSpecialCardUnlocked(character))
        {
            _background.material = _amazingBackgroundMaterial;
            Material _mat = new Material(_amazingMaterial);
            _mat.SetTexture("Texture2D_FAA9D2CE", Resources.Load<Texture2D>("Sprites/BigDraws/" + character + "_mask"));
            _mat.SetColor("Color_3F02EA66", new Color(0.65f, 0.21f, 0.144f));
            _mainImage.material = _mat;
            _foreground.enabled = true;
            //Material _foreMat = new Material(_amazingForegroundMaterial);
            //_foreMat = new Material
        }
        else
        {
            _foreground.enabled = false;
            _background.material = null;
            _mainImage.material = null;
        }
        
    }
    public void Init(int character, bool special)
    {
        _mainImage.sprite = Resources.Load<Sprite>("Sprites/BigDraws/" + character);
        _mainImage.overrideSprite = Resources.Load<Sprite>("Sprites/BigDraws/" + character);

        //_frame.sprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());
        //_frame.overrideSprite = Resources.Load<Sprite>("Sprites/Frames/" + UserDataController.GetCurrentFrame());

        _background.sprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (character / 4));
        _background.overrideSprite = Resources.Load<Sprite>("Sprites/Backgrounds/" + (character / 4));
        _background.preserveAspect = false;

        if (special)
        {
            _background.material = _amazingBackgroundMaterial;
            Material _mat = new Material(_amazingMaterial);
            _mat.SetTexture("Texture2D_FAA9D2CE", Resources.Load<Texture2D>("Sprites/BigDraws/" + character + "_mask"));
            _mat.SetColor("Color_3F02EA66", new Color(0.65f, 0.21f, 0.144f));
            _mainImage.material = _mat;
            _foreground.enabled = true;
            //Material _foreMat = new Material(_amazingForegroundMaterial);
            //_foreMat = new Material
        }
        else
        {
            _foreground.enabled = false;
            _background.material = null;
            _mainImage.material = null;
        }

    }

}
