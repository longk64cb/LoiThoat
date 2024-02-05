using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MovementController movementController;

    private bool canTeleport = true;
    private DateTime lastTeleTime;
    private MapEnum currentMap = MapEnum.Bedroom;
    public MapEnum CurrentMap => currentMap;

    public void MoveToMap(MapController map)
    {
        if ((DateTime.Now - lastTeleTime).TotalSeconds < 0.1f) return;

        AudioManager.I.Play(SoundID.door);

        LoadingController.I.Loading(() =>
        {
            if (map.MapType == MapEnum.Yard)
            {
                if (GameController.I.gameTime == GameTimeEnum.Day)
                {
                    AudioManager.I.Play(SoundID.yard_BG_1);
                    AudioManager.I.Play(SoundID.yard_BG_2);
                }
                else
                {
                    AudioManager.I.Play(SoundID.yard_BG_night);
                }
            }
            else
            {
                AudioManager.I.Stop(SoundID.yard_BG_1);
                AudioManager.I.Stop(SoundID.yard_BG_2);
                AudioManager.I.Stop(SoundID.yard_BG_night);
            }

            var spawnPos = map.GetSpawnPos(currentMap);
            if (spawnPos == null) return;
            transform.position = spawnPos.position;
            currentMap = map.MapType;
            lastTeleTime = DateTime.Now;
        });

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door") && collision.TryGetComponent<DoorController>(out DoorController door))
        {
            MoveToMap(door.TargetMap);
        }

        if (collision.CompareTag("Lake"))
        {
            GameController.I.OnArriveLake();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
