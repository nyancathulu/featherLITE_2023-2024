using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Send : MonoBehaviour
{
    public SlimeBehavior slimeCheck;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        slimeCheck.CollisionEvent(collision);
    }
}
