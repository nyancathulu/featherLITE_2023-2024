using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Bullet_Script : MonoBehaviour
{
    
    [SerializeField] LayerMask enemyPlayer;
    [SerializeField] LayerMask otherBullet;
    public LayerMask NoDestroyGround;
    public LayerMask boundary;
    public LayerMask destroyableGround;

    [SerializeField] float bulletvelocity;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] ParticleSystem BreakParticles;

    public Game_Ender_Script GameEnder;

    private void OnEnable()
    {
        rb.velocity = new Vector2(bulletvelocity * transform.right.x, 0);
    }

/*    void OnTriggerEnter2D(Collider2D Bulletcollision)
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
            rb.velocity = Vector2.zero;
            Tilemap tilemap = Bulletcollision.GetComponent<Tilemap>();
            Debug.Log(tilemap);
            Vector3Int position = new Vector3Int((int)(gameObject.transform.position.x + 0.5f), (int)gameObject.transform.position.y);
            tilemap.SetTile(position, null);
            //Debug.Log(Bulletcollision.gameObject);
            gameObject.SetActive(false);
        }
        if (layervalue == otherBullet.value)
        {
            gameObject.SetActive(false);
        }
        if (layervalue == boundary)
        {
            gameObject.SetActive(false);
        }
    }*/

/*    void OnCollisionEnter2D(Collision2D Bulletcollision)
    {
        Debug.Log(Bulletcollision.gameObject);
        float layervalue = Mathf.Pow(2, Bulletcollision.gameObject.layer);

       

    }*/

    void OnTriggerEnter2D(Collider2D Bulletcollision)
    {
        int layer = Bulletcollision.gameObject.layer;

        if ((enemyPlayer & (1 << layer)) != 0)
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
        if ((destroyableGround & (1 << layer)) != 0)
        {
            rb.velocity = Vector2.zero;
            Tilemap tilemap = Bulletcollision.gameObject.GetComponent<Tilemap>();
            Vector3 position = Vector3.zero;
            if (gameObject.transform.localEulerAngles.y < 0.1f) position = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y, 0);
            if (gameObject.transform.localEulerAngles.y > 179.9f) position = new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, 0);
            //Debug.Log(position);
            tilemap.SetTile(tilemap.WorldToCell(position), null);
            //Debug.Log(Bulletcollision.gameObject);
            BulletDie();
        }
        if (((otherBullet | boundary | NoDestroyGround) & (1 << layer)) != 0)
        {
            BulletDie();
        }
/*        if ((boundary & (1 << layer)) != 0)
        {
            gameObject.SetActive(false);
        }*/
    }
    void BulletDie()
    {
        BreakParticles.Play();
        gameObject.SetActive(false);
    }
}
