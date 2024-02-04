using Everest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] MapEnum mapType;
    public MapEnum MapType => mapType;

    #region Time
    [SerializeField] SpriteRenderer mapSprite;
    [SerializeField] Sprite[] arrMapSprite;

    [SerializeField] GameObject dayOG;
    [SerializeField] GameObject nightGO;
    [SerializeField] TimeSwitchableGameObject[] timeSwitchableGameObjects;

    public void OnChangeGameTime(GameTimeEnum gameTime)
    {
        mapSprite.sprite = arrMapSprite[(int)gameTime];
        if (dayOG != null) dayOG?.SetActive(gameTime == GameTimeEnum.Day);
        if (nightGO != null) nightGO.SetActive(gameTime == GameTimeEnum.Night);

        foreach (var obj in timeSwitchableGameObjects)
        {
            obj.OnChangeTime(gameTime);
        }
    }
    #endregion

    #region Spawn
    [SerializeField] EnumResource<MapEnum, Transform> spawnPos;
    public Transform GetSpawnPos(MapEnum from)
    {
        return spawnPos[from];
    }
    #endregion
}
