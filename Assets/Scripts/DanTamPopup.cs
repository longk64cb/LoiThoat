using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class DanTamPopup : MonoBehaviour
{
    private const string IS_DAN_TAM_CLAIMED = "dan_tam";

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject buttons;

    public UnityEvent onAchiveDanTam;

    private void OnEnable()
    {
        buttons.SetActive(PlayerPrefs.GetInt(IS_DAN_TAM_CLAIMED, 0) == 0);
    }

    public void OnClaim()
    {
        PlayerPrefs.SetInt(IS_DAN_TAM_CLAIMED, 1);
        Close();
        onAchiveDanTam?.Invoke();
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
