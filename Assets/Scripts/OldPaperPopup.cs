using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OldPaperPopup : MonoBehaviour
{
    private const string IS_OLD_PAPER_CLAIMED = "old_paper";

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject buttons;

    public bool isGet = false;
    public UnityEvent onAchiveOldPaper;

    private void OnEnable()
    {
        buttons.SetActive(!isGet);
    }

    public void OnClaim()
    {
        isGet = true;
        Close();
        onAchiveOldPaper?.Invoke(); 
    }

    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1f, 0.5f).From(0f);
    }

    public void Close()
    {
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
}
