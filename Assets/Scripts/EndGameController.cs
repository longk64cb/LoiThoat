using DG.Tweening;
using Everest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] Text deadText;
    [SerializeField] CanvasGroup tryAgain;

    public void Show()
    {
        tryAgain.gameObject.SetActive(false);
        deadText.gameObject.SetActive(true);
        gameObject.SetActive(true);

        canvasGroup.DOFade(1f, 1f).From(0f).OnComplete(() =>
        {
            AudioManager.I.Play(SoundID.die);
        });

        this.Delay(2f, () =>
        {
            deadText.DOFade(0f, 0.75f).OnComplete(() => deadText.gameObject.SetActive(false));
            tryAgain.gameObject.SetActive(true);
            tryAgain.DOFade(1f, 0.75f).SetDelay(0.75f);
        });
    }
}
