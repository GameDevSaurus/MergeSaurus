using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDino", menuName = "CreadorSO/Dino", order = 2)]
public class SOADino : ScriptableObject
{
    public int dinoType;
    public Sprite image;
}