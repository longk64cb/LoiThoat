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

    public void MoveToMap(MapController map)
    {
        if ((DateTime.Now - lastTeleTime).TotalSeconds < 0.1f) return;

        LoadingController.I.Loading(() =>
        {
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
