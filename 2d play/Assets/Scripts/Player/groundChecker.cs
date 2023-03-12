using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundChecker : MonoBehaviour
{
    public bool isOnGround;
    void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject);
        isOnGround = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("out");
        isOnGround = false;
    }
}
