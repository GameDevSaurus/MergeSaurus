using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class DinosaurInstance : MonoBehaviour
{
    int _dinoType;
    int _cellIndex;
    [SerializeField]
    GameObject _otherCell;
    bool _working;
    [SerializeField]
    Animator _dinoAnimator;
    [SerializeField]
    SpriteRenderer _dinoSprite;
    public void StartWorking()
    {
        _working = true;
        GameEvents.WorkDino.Invoke();
        if (_dinoSprite != null)
        {
            _dinoSprite.color = new Color(95f / 255f, 95f / 255f, 95f / 255f, 1f);
        }
        
        if(_dinoAnimator != null)
        {
            _dinoAnimator.speed = 0f;
        }
        else
        {
            UnityArmatureComponent armature = GetComponentInChildren<UnityArmatureComponent>();
            if (armature != null)
            {
                armature.animation.timeScale = 0;
            }
        }
        
    }
    public void StopWorking()
    {
        _working = false;
        if (_dinoSprite != null)
        {
            _dinoSprite.color = Color.white;
        }
        
        if (_dinoAnimator != null)
        {
            _dinoAnimator.speed = 1f;
        }
        else
        {
            UnityArmatureComponent armature = GetComponentInChildren<UnityArmatureComponent>();
            if (armature != null)
            {
                armature.animation.timeScale = 1f;
            }
        }
    }

    public void SetCell(int nCell)
    {
        _cellIndex = nCell;
    }
    public void SetDino(int nDino)
    {
        _dinoType = nDino;
    }
    public int GetDinosaur()
    {
        return _dinoType;
    }
    public int GetCellNumber()
    {
        return _cellIndex;
    }
    public bool IsWorking()
    {
        return _working;
    }
}
