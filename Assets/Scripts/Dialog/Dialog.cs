using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public CharacterEnum character;
    public CharacterDialogPos dialogPos;

    [TextArea(3, 5)]
    public string dialogText;

    public Dialog nextDialog;

    [SerializeField]
    public DialogChoice[] dialogChoices;
}

[Serializable]
public struct DialogChoice
{
    public string text;
    public Dialog nextDialog;
}

public enum CharacterEnum
{
    None = 0,
    Rabbit = 1,
    Buffalo = 2,
    Fox = 3,
    Deer = 4
}

public enum CharacterDialogPos
{
    Left,
    Right
}
