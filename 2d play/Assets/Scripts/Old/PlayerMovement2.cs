using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour
{
    public DebugLogger logger;
    [Space]
    public Rigidbody2D rb;
    [Space(5)]
    [Header("Factors")]
    //  [SerializeField]
    public float topSpeed;
    // [SerializeField]
    [Range(0.1f, 1)]
    public float accelRate;
    [Range(0.1f, 1)]
    public float decelRate;
    public float targetvelocity;
    public float JumpSpeed;
    public float gravity;
    [Header("References")]
    public GameObject groundChecker;
    public LayerMask ground;
    [Header("Inputs")]
    [Space]
    // [SerializeField]
    public InputActionReference fire;
    // [SerializeField]
    public InputActionReference look;
    //[SerializeField]
    public InputActionReference movement;
    //[SerializeField]
    public InputActionReference jump;



    private Vector2 movementinput;
    private float finalgravity;
    private float finalAccelRate;
    private bool isOnGround;
    void Update()
    {
        //movement
        movementinput = movement.action.ReadValue<Vector2>();
        if (movementinput.x != 0)
        {
            finalAccelRate = accelRate;
        }
        if (movementinput.x == 0)
        {
            finalAccelRate = decelRate;
        }
        logger.Log(finalAccelRate);


        //jump
        isOnGround = groundChecker.GetComponent<groundChecker>().isOnGround;

        if (isOnGround && jump.action.triggered)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
        }
        if (jump.action.IsPressed() && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpSpeed * 0.5f);
        }


        //gravity
        finalgravity = gravity;
    }
    void FixedUpdate()
    {

        //movement
        targetvelocity = (movementinput.x * topSpeed) - (rb.velocity.x);
        //logger.Log(accelRate * targetvelocity * Vector2.right);
        rb.AddForce(finalAccelRate * targetvelocity * Vector2.right, ForceMode2D.Impulse);




        //gravity
        rb.AddForce(Vector3.down * finalgravity, ForceMode2D.Impulse);
    }
}
