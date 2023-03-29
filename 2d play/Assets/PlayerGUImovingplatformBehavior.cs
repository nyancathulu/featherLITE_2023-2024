using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUImovingplatformBehavior : MonoBehaviour
{
    public GameObject player;
    public bool smoothfollow;
    public float smoothfollowstep;
    void Start()
    {
        smoothfollow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (smoothfollow)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, smoothfollowstep);
        }
    }
}
