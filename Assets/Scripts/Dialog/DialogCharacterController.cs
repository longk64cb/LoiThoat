using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCharacterController : MonoBehaviour
{
    private const float ANIM_TIME = 0.5f;
    [SerializeField] CanvasGroup canvasGroup;

    public Sequence Show(CharacterDialogPos pos)
    {
        if (gameObject.activeInHierarchy)
        {
            canvasGroup.alpha = 1;
            return null;
        }

        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOLocalMoveX(pos == CharacterDialogPos.Left ? -100f : 100f, ANIM_TIME).From());
        sequence.Join(canvasGroup.DOFade(1f, ANIM_TIME).From(0f));
        return sequence;
    }

    public Tween Hide()
    {
        if (!gameObject.activeInHierarchy) return null;

        return canvasGroup.DOFade(0f, ANIM_TIME).OnComplete(() => gameObject.SetActive(false));
    }
}
