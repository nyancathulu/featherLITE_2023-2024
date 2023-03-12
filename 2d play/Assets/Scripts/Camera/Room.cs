using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject VirtualCam;
    public int cameraOrderNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        VirtualCam.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        VirtualCam.SetActive(false);
    }
}
