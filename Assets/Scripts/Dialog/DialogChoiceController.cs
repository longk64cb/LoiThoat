using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogChoiceController : MonoBehaviour
{
    [SerializeField] private Text choiceText;
    [SerializeField] CanvasGroup canvasGroup;
    public Dialog targetDialog;
    public string choiceStr;

    public void Setup(string text, Dialog targetDialog)
    {
        choiceStr = text;
        choiceText.text = text.ToUpper();
        this.targetDialog = targetDialog;

        gameObject.SetActive(true);
        canvasGroup.DOFade(1f, 0.5f).From(0f);
    }
}
