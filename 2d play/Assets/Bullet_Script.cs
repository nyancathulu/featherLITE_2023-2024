using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    
    [SerializeField] LayerMask enemyPlayer;
    [SerializeField] LayerMask otherBullet;
    public LayerMask ground;

    [SerializeField] float bulletvelocity;

    [SerializeField] Rigidbody2D rb;

    public Game_Ender_Script GameEnder;

    private void OnEnable()
    {
        rb.velocity = new Vector2(bulletvelocity * transform.right.x, 0);
    }

    void OnTriggerEnter2D(Collider2D Bulletcollision)
    {

        float layervalue = Mathf.Pow(2, Bulletcollision.gameObject.layer);

        if (layervalue == enemyPlayer.value)
        {
            if (Bulletcollision.gameObject.CompareTag("Red Player"))
            {
                GameEnder.EndGame(0, gameObject);
            }
            if (Bulletcollision.gameObject.CompareTag("Blue Player"))
            {
                GameEnder.EndGame(1, gameObject);
            }
            //gameObject.SetActive(false);
            rb.velocity = Vector2.zero;
        }
        if (layervalue == ground.value)
        {
            //Debug.Log(Bulletcollision.gameObject);
            gameObject.SetActive(false);
        }
        if (layervalue == otherBullet.value)
        {
            gameObject.SetActive(false);
        }
    }
}
