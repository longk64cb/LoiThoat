using DG.Tweening;
using Everest;
using Everest.CustomEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementUIController : MonoBehaviour
{
    [SerializeField] Image notiImg;

    [Button]
    public void Test()
    {
        Show(notiImg.sprite);
    }
    
    public void Show(Sprite sprite)
    {
        gameObject.SetActive(true);
        notiImg.sprite = sprite;

        Sequence sequence = DOTween.Sequence();
        sequence.Join(notiImg.DOFade(1f, 0.5f).From(0f));
        sequence.Join(transform.DOMoveY(-50f, 0.5f).From().SetRelative());
        sequence.AppendInterval(1f);
        sequence.Append(notiImg.DOFade(0f, 0.5f));
        sequence.OnComplete(() => gameObject.SetActive(false));
    }
}
