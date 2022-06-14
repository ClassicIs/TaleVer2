using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[Serializable]
[CreateAssetMenu(fileName = "DialogueCharacter", menuName = "Dialogue/New Character")]
public class Character : ScriptableObject
{
    public string charName;
    public Color charColor;
    public Sprite theCharSpr;
}
