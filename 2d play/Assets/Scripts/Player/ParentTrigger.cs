using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTrigger : MonoBehaviour
{
    public GameObject player;
    public MovingPlatform platform;
    public float parentGoneTime;
    bool isOnPlatform;
    private void Start()
    {
        isOnPlatform = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        isOnPlatform = true;
       // Debug.Log("in");
        player.transform.SetParent(platform.gameObject.transform);
        platform.StartMove();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnPlatform = false;
        StartCoroutine(RemoveParent(parentGoneTime));
    }
    public IEnumerator RemoveParent(float t)
    {
        float timer = t;
        while (true)
        {
            if (timer < 0) break;
            if (isOnPlatform) yield break;
            timer -= Time.deltaTime;
            yield return 0;
        }
        //Debug.Log("ahahahahahahahaahah");
        if ((!isOnPlatform) && (player.transform.parent == platform.gameObject.transform))
        {
            Debug.Log("ahahahahahahahaahah");
            player.transform.SetParent(null);
        }
        yield break;
    }
}
