using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlimeBehavior : MonoBehaviour
{
    public GameObject player;
    public Vector2 playervelocity;
    public Vector2 sideForceVector;
    public InputActionReference jump;
    [Range(0,1)]
    public float bouncinessfactor;
    [Range(1,2)]
    public float nothingBouncinessOffset;
    public float slimeLeniency;
    float mayslime;
    public int targetforce;
    public delegate void SlimeEvent(bool isSide, Vector2 velocity);
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        if (!player.GetComponent<PlayerMovement>().isSlimed | (-collision.GetContact(0).normal * playervelocity).magnitude < bounceThreshold) jumpcount = targetforce/2;
        if (player.GetComponent<PlayerMovement>().isSlimed | (-collision.GetContact(0).normal * playervelocity).magnitude > bounceThreshold)
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
        if ((-collision.GetContact(0).normal * playervelocity).magnitude > bounceThreshold)
        {
            //if (collision.GetContact(0).normal == Vector2.up) Debug.Log("up");
            //Debug.Log(collision.GetContact(0).normal.y > 0);
            if (Mathf.Abs(-collision.GetContact(0).normal.y) >= Mathf.Abs(-collision.GetContact(0).normal.x)) 
            {
                
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
                player.GetComponent<Rigidbody2D>().AddForce(-collision.GetContact(0).normal * (targetforce - jumpcount), ForceMode2D.Impulse);
                if (OnSlime != null) OnSlime(false, Vector2.zero);
            }

            if (Mathf.Abs(-collision.GetContact(0).normal.y) < Mathf.Abs(-collision.GetContact(0).normal.x)) 
            {
                //player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
                //player.GetComponent<Rigidbody2D>().velocity = new Vector2(sideForceVector.x * collision.GetContact(0).normal.x, sideForceVector.y);
                Vector2 sideVelocity = Vector2.Reflect(playervelocity, -collision.GetContact(0).normal); //new Vector2(sideForceVector.x * -collision.GetContact(0).normal.x, sideForceVector.y);
                if (OnSlime != null) OnSlime(true, sideVelocity);
            }
            
            
        }
      
    }
}
