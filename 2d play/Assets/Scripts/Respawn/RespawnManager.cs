using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List<CheckPoints> CheckpointsList;
    Transform respawnPoint;
    int currentRespawn = 0;
    public delegate void DeathEvent(Vector2 p);
    public static event DeathEvent OnDeath;
    public void SetCheckpoint(int CheckPointNumber)
    {
        if (CheckPointNumber - 1 >= currentRespawn)
        {
            respawnPoint = CheckpointsList[CheckPointNumber - 1].GetComponent<CheckPoints>()._respawnPoint;
            currentRespawn = CheckPointNumber - 1;
        }
        
    }
    public void SendEvent()
    {
        if (OnDeath != null) OnDeath(respawnPoint.position);
    }
}
