using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slideChecker : MonoBehaviour
{
    public bool canSlide;
    void OnTriggerStay2D(Collider2D collision)
    {
        canSlide = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        canSlide = false;
    }
}
