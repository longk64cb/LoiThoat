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

    public void Loading(Action onLoadAction = null, Action onLoadingFinishAction = null)
    {
        loadingImg.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(loadingImg.DOFade(1f, 0.5f).From(0f).OnComplete(() => onLoadAction?.Invoke()));
        sequence.Append(loadingImg.DOFade(0f, 0.5f));
        sequence.OnComplete(() => 
        {
            loadingImg.gameObject.SetActive(false);
            onLoadingFinishAction?.Invoke();
        });
    }
}
