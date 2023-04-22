using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject playerUI;
    public MovingPlatform platform;
    public float parentGoneTime;
    public float radius;
    public static bool isOnPlatform;
    public LayerMask playermask;
    bool parented;
    private void Start()
    {
        isOnPlatform = false;
    }
    private void Update()
    {
       // Debug.Log(gameObject.name + " " + isOnPlatform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        isOnPlatform = true;
        parented = true;
       // Debug.Log("in");
        player.transform.SetParent(platform.gameObject.transform);
     /*   playerUI.transform.SetParent(null);
        playerUI.GetComponent<PlayerGUImovingplatformBehavior>().smoothfollow = true;*/
        player.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
        platform.StartMove();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnPlatform = false;

        StartCoroutine(RemoveParent(parentGoneTime));
    }
    public IEnumerator RemoveParent(float t)
    {
       // float timer = t;
        while (Physics2D.OverlapCircle(platform.transform.position, radius, playermask))
        {
            /* if (timer < 0) break;
             if (isOnPlatform) yield break;
             timer -= Time.deltaTime;
             yield return 0;*/
            if (isOnPlatform) yield break;
            yield return 0;
        }
        ///Debug.Log("ahahahahahahahaahah");
        if ((!isOnPlatform) && (player.transform.parent == platform.gameObject.transform))
        {
            Debug.Log("ahahahahahahahaahah");
            parented = false;
            player.transform.SetParent(null);
           /* playerUI.GetComponent<PlayerGUImovingplatformBehavior>().smoothfollow = false;
            playerUI.transform.SetParent(player.transform);*/
            playerUI.transform.localPosition = Vector2.zero;
        }
        if (!isOnPlatform) player.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        yield break;
    }
}
