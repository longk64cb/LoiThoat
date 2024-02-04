using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitchableGameObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite daySprite;
    [SerializeField] Sprite nightSprite;

    public void OnChangeTime(GameTimeEnum gameTime)
    {
        spriteRenderer.sprite = gameTime == GameTimeEnum.Day ? daySprite : nightSprite;
    }
}
