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
    public bool isGet;

    private void OnEnable()
    {
        buttons.SetActive(!isGet);
    }

    public void OnClaim()
    {
        isGet = true;
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
