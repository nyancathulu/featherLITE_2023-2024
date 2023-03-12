using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoomSystem : MonoBehaviour
{
    public int StartingRoom;
    public List<GameObject> VirtualCameras;
    [Space(10)]
    [Header("Don't Edit")]
    public GameObject targetObject;



    void Start()
    {
       // ChangeCamera(StartingRoom);
    }


    /*public void ChangeCamera(int targetNumber)
    {
        for (int i = 0; i < VirtualCameras.Count; i++)
        {
            if (i == targetNumber - 1)
            {
                VirtualCameras[i].SetActive(true);
            }
            else
            {
                VirtualCameras[i].SetActive(false);
            }
        }
    }*/
}
