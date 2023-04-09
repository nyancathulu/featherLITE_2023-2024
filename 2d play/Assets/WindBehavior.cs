using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBehavior : MonoBehaviour
{
    public float fanForce;
    public float forceVariance;
    public GameObject parent;
    float appliedForce;
    float distance;
    Rigidbody2D player;
    bool fanning;
    float angle;
    private void Start()
    {
        angle = (parent.transform.localEulerAngles.z - 270) * Mathf.Deg2Rad;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Rigidbody2D>();
        fanning = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        fanning = false;
    }
    private void FixedUpdate()
    {
        Debug.Log(Mathf.Cos(angle));
        if (fanning)
        {
            Debug.Log(appliedForce);
            distance = Vector2.Distance(player.position, new Vector2(transform.position.x, transform.position.y));
            appliedForce = fanForce / (1 + distance * distance);
            player.AddForce(Vector2.right * Mathf.Cos(angle) * appliedForce, ForceMode2D.Impulse);
            player.AddForce(Vector2.up * Mathf.Sin(angle) * appliedForce, ForceMode2D.Impulse);
        }
        
    }
}
