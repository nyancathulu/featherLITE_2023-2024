using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.parent = collision.transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player.transform.parent = collision.transform;
    }
}
