using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlimeBehavior : MonoBehaviour
{
    public GameObject player;
    public Vector2 playervelocity;
    Vector2 returnVelocity;
    public InputActionReference jump;
    [Range(0,1)]
    public float bouncinessfactor;
    [Range(1,2)]
    public float nothingBouncinessOffset;
    public float slimeLeniency;
    float jumpFactor = 1;
    bool endCoroutines;
    bool coroutineRunning;
    float mayslime;
    public int targetforce;
    public delegate void SlimeEvent();
    public static event SlimeEvent OnSlime;
    [Range(0,10)]
    public float bounceThreshold;
    int jumpcount;
    public int upfactor;
    public int downfactor;
    private void FixedUpdate()
    {
        playervelocity = player.GetComponent<Rigidbody2D>().velocity;
    }
    private void Update()
    {
        if (jump.action.triggered)
        {
            mayslime = slimeLeniency;
        }
        else
        {
            if (mayslime > 0) mayslime -= Time.deltaTime;
        }
        //Debug.Log(mayslime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        /* if (Mathf.Abs(playervelocity.x) > Mathf.Abs(playervelocity.y))
         {
             player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-playervelocity.x * bouncinessfactor, 0), ForceMode2D.Impulse);
         }
         else
         {
             player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -playervelocity.y * bouncinessfactor), ForceMode2D.Impulse);
         }*/
        
        if (mayslime > 0) jumpFactor = 1/bouncinessfactor;
        else jumpFactor = 1;


        if (!player.GetComponent<PlayerMovement>().isSlimed | (collision.GetContact(0).normal * playervelocity).magnitude < bounceThreshold) jumpcount = targetforce/2;
        if (player.GetComponent<PlayerMovement>().isSlimed | (collision.GetContact(0).normal * playervelocity).magnitude > bounceThreshold)
        {
            if (mayslime > 0)
            {
                if (jumpcount - downfactor > 0) jumpcount -= downfactor;
                else jumpcount = 0;
            }
            if (mayslime < 0)
            {
                if (jumpcount + upfactor < targetforce) jumpcount += upfactor;
                else jumpcount = targetforce;
            }
                
        }
        //Debug.Log(((targetforce - Mathf.Abs(playervelocity.y)) * bouncinessfactor));
        if ((collision.GetContact(0).normal * playervelocity).magnitude > bounceThreshold)
        {
            //Debug.Log("jumping");
            /*if (mayslime > 0) player.GetComponent<Rigidbody2D>().AddForce(collision.GetContact(0).normal * new Vector2(2 * Mathf.Abs(playervelocity.x) + ((targetforce - Mathf.Abs(playervelocity.x)) * bouncinessfactor), 2 * Mathf.Abs(playervelocity.y) + ((targetforce - Mathf.Abs(playervelocity.y)) * bouncinessfactor)), ForceMode2D.Impulse);
            else player.GetComponent<Rigidbody2D>().AddForce(collision.GetContact(0).normal * new Vector2(Mathf.Abs(playervelocity.x) - nothingBouncinessOffset, Mathf.Abs(playervelocity.y) - nothingBouncinessOffset), ForceMode2D.Impulse);*/
            //player.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Abs(playervelocity.x), Mathf.Abs(playervelocity.y)) + collision.GetContact(0).normal * targetforce / jumpcount, ForceMode2D.Impulse);
            //player.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity * collision.GetContact(0).normal;
            //player.GetComponent<Rigidbody2D>().AddForce(collision.GetContact(0).normal * new Vector2((targetforce - jumpcount) + collision.GetContact(0).relativeVelocity.x, (targetforce - jumpcount) + collision.GetContact(0).relativeVelocity.y), ForceMode2D.Impulse);
            //player.GetComponent<Rigidbody2D>().AddForce(collision.GetContact(0).normal * collision.GetContact(0).normalImpulse, ForceMode2D.Impulse);
            if (collision.GetContact(0).normal == Vector2.up | collision.GetContact(0).normal == Vector2.down) player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
            if (collision.GetContact(0).normal == Vector2.right | collision.GetContact(0).normal == Vector2.left) player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
            player.GetComponent<Rigidbody2D>().AddForce(collision.GetContact(0).normal * (targetforce - jumpcount), ForceMode2D.Impulse);
        }
        //player.GetComponent<Rigidbody2D>().AddForce(new Vector2(-playervelocity.x * bouncinessfactor, -playervelocity.y * bouncinessfactor), ForceMode2D.Impulse);
        if (OnSlime != null) OnSlime();
        //if (coroutineRunning) endCoroutines = true;
        //StartCoroutine(slimeCoroutine(0.5f, -collision.GetContact(0).normal));
        /*Debug.Log("jumpcount" + jumpcount);
        Debug.Log(collision.GetContact(0).normal * new Vector2(Mathf.Abs(playervelocity.x) + targetforce / jumpcount, Mathf.Abs(playervelocity.y) + targetforce / jumpcount));*/
        //Debug.Log(collision.GetContact(0).normal * new Vector2(Mathf.Abs(playervelocity.x) + targetforce / jumpcount, Mathf.Abs(playervelocity.y) + targetforce / jumpcount));
        Debug.Log("jumpcount" + jumpcount);

    }
    public IEnumerator slimeCoroutine(float t, Vector2 forceDir)
    {
        float timer = t;
        Vector2 playervelocity = player.GetComponent<Rigidbody2D>().velocity;
        player.GetComponent<PlayerMovement>().sideinput = false;
        coroutineRunning = true;
        while (timer >= 0)
        {
            Debug.Log(gameObject.name);
            if (endCoroutines) break;
            player.GetComponent<Rigidbody2D>().AddForce(forceDir * playervelocity * 5, ForceMode2D.Impulse);
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        player.GetComponent<PlayerMovement>().sideinput = true;
        endCoroutines = false;
        coroutineRunning = false;
        yield break;
    }
}
