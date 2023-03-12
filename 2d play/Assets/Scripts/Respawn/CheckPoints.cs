using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public RespawnManager respawnmanager;
    public int CheckPointNumber;
    public Transform _respawnPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.gameObject.tag == "Player")
        {
            respawnmanager.SetCheckpoint(CheckPointNumber);
        }
        
    }
}
