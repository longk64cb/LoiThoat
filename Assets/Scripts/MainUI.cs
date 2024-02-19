using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] Image mainUIImg;
    [SerializeField] Text taskText;

    [SerializeField] Sprite dayMainSprite;
    [SerializeField] Sprite nightMainSprite;

    [SerializeField] Sprite dayMenuSprite;
    [SerializeField] Sprite nightMenuSprite;

    private Tween shakeTween;

    public void SetTaskText(string text)
    {
        taskText.text = text;
    }

    public void OnSkipTime()
    {
        if (GameController.I.gameTime == GameTimeEnum.Night) return;

        if (GameController.I.gameData.characterTalked.Count(x => x.Value) < 3)
        {
            shakeTween?.Kill(true);
            shakeTween = taskText.transform.DOShakePosition(0.5f, 5);
            return;
        }

        GameController.I.SetGameTime(GameTimeEnum.Night);
        mainUIImg.sprite = nightMainSprite;
        menuImg.sprite = nightMenuSprite;
    }

    [SerializeField] RectTransform menuRect;
    [SerializeField] Image menuImg;
    [SerializeField] Button closeMenuBtn;
    private bool isShow;
    public void OnToggleMenu()
    {
        if (!isShow)
        {
            menuRect.gameObject.SetActive(true);
            closeMenuBtn.gameObject.SetActive(true);
            menuRect.DOAnchorPosY(0f, 0.5f);
            isShow = true;
        }
        else
        {
            isShow = false;
            menuRect.DOAnchorPosY(445f, 0.5f).OnComplete(() =>
            {
                menuRect.gameObject.SetActive(false);
                closeMenuBtn.gameObject.SetActive(false);
            });
        }
    }
}
