using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCheck_Disable : MonoBehaviour
{
    public Collider2D DeathCollider;

    public void DeathColliderDie()
    {
        DeathCollider.enabled = false;
    }
}
