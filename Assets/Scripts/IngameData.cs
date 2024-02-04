using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IngameData : MonoBehaviour
{
    public bool isDanTamClaimed;
    public bool isOldPaperClaim;

    public CharacterTalkedDictionary characterTalked = new()
    {
        {CharacterEnum.Buffalo, false},
        {CharacterEnum.Fox, false},
        {CharacterEnum.Deer, false},
    };
}

[Serializable]
public class CharacterTalkedDictionary : Dictionary<CharacterEnum, bool> { }
