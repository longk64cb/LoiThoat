using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup; 

    [SerializeField] Sprite[] arrTutorialSprite;
    [SerializeField] Image tutorialImage;

    [SerializeField] Transform[] arrPageCount;
    [SerializeField] Transform point;

    private int currentPage;

    public void NextPage()
    {
        if (currentPage >= arrTutorialSprite.Length - 1)
            currentPage = 0;
        else currentPage++;

        LoadPage();
    }

    public void PrevPage()
    {
        if (currentPage <= 0)
            currentPage = arrTutorialSprite.Length - 1;
        else currentPage--;

        LoadPage();
    }

    public void LoadPage()
    {
        tutorialImage.sprite = arrTutorialSprite[currentPage];
        point.parent = arrPageCount[currentPage];
        point.localPosition = Vector3.zero;
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
