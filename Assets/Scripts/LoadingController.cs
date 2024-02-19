using DG.Tweening;
using Everest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : Singleton<LoadingController>
{
    [SerializeField] Image loadingImg;

    public void Loading(Action onLoadAction = null, Action onLoadingFinishAction = null, float duration = 1f)
    {
        loadingImg.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(loadingImg.DOFade(1f, duration / 2).From(0f).OnComplete(() => onLoadAction?.Invoke()));
        sequence.Append(loadingImg.DOFade(0f, duration / 2));
        sequence.OnComplete(() => 
        {
            loadingImg.gameObject.SetActive(false);
            onLoadingFinishAction?.Invoke();
        });
    }
}
