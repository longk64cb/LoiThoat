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
    }
}
