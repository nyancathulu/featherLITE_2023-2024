using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class FallingPlatformBehavior : MonoBehaviour
{
    public float crumbleTime;
    public float respawnTime;
    public LayerMask playerMask;
    public TextMeshProUGUI Temptext;
    float timer;
    bool isCrumbling;
    private void Start()
    {
        isCrumbling = false;
        Temptext.text = crumbleTime.ToString();
    }
    private void OnEnable()
    {
        RespawnManager.OnDeath += PlatformRespawn;
    }
    private void OnDisable()
    {
        RespawnManager.OnDeath -= PlatformRespawn;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       /* Debug.Log(collision.gameObject.layer);
        Debug.Log(collision.gameObject.layer == playerMask);*/
        if (collision.gameObject.layer == 13)
        {
           
            if (!isCrumbling) StartCoroutine(PlatformCrumble(crumbleTime, respawnTime));
        }
    }
    IEnumerator PlatformCrumble(float destroyTime, float respawningTime)
    {
        isCrumbling = true;
        float timer = destroyTime;
        while (timer > 0 && isCrumbling)
        {
            Temptext.text = ((float)Math.Round((double)timer, 2)).ToString();
            timer -= Time.deltaTime;
            yield return 0;
        }
        if (isCrumbling)
        {
            Temptext.text = "0";
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
      
        float respawnTimer = respawningTime;
        while (respawnTimer > 0 && isCrumbling)
        {
            Debug.Log(respawnTimer);
            respawnTimer -= Time.deltaTime;
            yield return 0;
        }
        if (isCrumbling) PlatformRespawn(Vector2.zero);
        yield break;
    }
    void PlatformRespawn(Vector2 p)
    {
        isCrumbling = false;
        Temptext.text = crumbleTime.ToString();
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
