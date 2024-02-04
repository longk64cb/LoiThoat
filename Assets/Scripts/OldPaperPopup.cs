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

    public UnityEvent onAchiveOldPaper;

    private void OnEnable()
    {
        buttons.SetActive(PlayerPrefs.GetInt(IS_OLD_PAPER_CLAIMED, 0) == 0);
    }

    public void OnClaim()
    {
        PlayerPrefs.SetInt(IS_OLD_PAPER_CLAIMED, 1);
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
