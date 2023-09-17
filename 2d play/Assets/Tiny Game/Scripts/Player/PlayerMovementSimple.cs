using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerMovementSimple : MonoBehaviour
{
    public DebugLogger logger;
    [Space]
    public Rigidbody2D rb;
    [Space(5)]
    [Header("Factors")]
  //  [SerializeField]
    public float topSpeed;
    // [SerializeField]
    //[Range(0.1f,1)]
    public float accelRate;
    //[Range(0.1f, 1)]
    public float decelRate;
    public float targetvelocity;
    public float jumpspeed;
    public float groundcheckdistance;
    public float gravity;
    [Range(1,30)]
    public float wallSlidingSpeed;
   // [Range(1, 2)]
    public float gravitydownscale;
   // [Range(1,10)]
    public float cutoffgravitydownscale;
    [Range(0.1f, 1f)]
    public float transitionpoint;
    public int jumpwaitframes;
    [Range(0.1f, 1)]
    public float SquashAndStretchRANGE;
    [Range(0.0001f, 1)]
    public float SquashAndStretchFACTOR;
    [Range(0, 2)]
    public float CoyoteTimeSeconds;
    //[Range(0.1f,1)]
    public float AirBrakeaccelRate;
    [Range(0,100)]
    public float terminalvelocity;
    /*    [Range(0, 1)]
        public float wallSlideLeniency;
        public float wallJumpTime;
        public Vector2 wallJumpVector;
        public int WallJumpBufferFrames;
        public float WallJumpCoyoteTime;
        [Range(0, 1)]
        */
    public float WallJumpMoveBackLenciency;
    [Range(0,1)]
    public float WallJumpAceelRate;
    [Header("Recommended 0.2")]
    public float wallJumpingTime = 0.2f;
    [Header("Recommended 0.4")]
    public float wallJumpingDuration = 0.4f;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);
    public float respawnDelaySeconds;
    [Space(5)]
    [Header("References")]
    public GameObject groundChecker;
    public LayerMask ground;
    public GameObject wallChecker;
    public LayerMask wall;
    public GameObject LAYERChecker;
    [Space(10)]
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
    private float finalaccelrate;
    float MayJump;
    bool JumpFlag;
    bool isJumping;
    float OGscale;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    public bool sideinput;
    bool isDying;


    //chaching
    groundChecker cachedGroundCheck;

    private bool isOnGround()
    {
        return cachedGroundCheck.isOnGround;
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallChecker.transform.position, 0.2f, wall);
    }

    private float facingDirection()
    {
        if (transform.rotation == Quaternion.identity)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    void OnEnable()
    {
        RespawnManager.OnDeath += Die;
    }
    void OnDisable()
    {
        RespawnManager.OnDeath -= Die;
    }
    void Start()
    {
        sideinput = true;
        OGscale = gameObject.transform.localScale.x;
        cachedGroundCheck = groundChecker.GetComponent<groundChecker>();
    }
    void Update()
    {

        //checks

        
        if (!isJumping)
        {
            if (isOnGround())
            {
                MayJump = CoyoteTimeSeconds;
            }
            if (!isOnGround())
            {
                MayJump -= Time.deltaTime;
            }
        }
       if (isJumping)
        {
            if (isOnGround())
            {
                MayJump = CoyoteTimeSeconds;
            }
            if (!isOnGround())
            {
                MayJump = -1f;
            }
        }

        
        //movement
        if (sideinput) movementinput = movement.action.ReadValue<Vector2>();
        if (isWallJumping)
        {
            if (wallJumpingDirection > 0)
            {
                movementinput = new Vector2(Mathf.Clamp(movement.action.ReadValue<Vector2>().x, WallJumpMoveBackLenciency, 1f), movement.action.ReadValue<Vector2>().y);
            }
            if (wallJumpingDirection < 0)
            {
                movementinput = new Vector2(Mathf.Clamp(movement.action.ReadValue<Vector2>().x, -1f, -WallJumpMoveBackLenciency), movement.action.ReadValue<Vector2>().y);
            }
        }

        //jump
        if (!isWallSliding)
        {
            if (jump.action.triggered)
            {

                StartJumpSequence();

            }
        }

       
        WallSlide();
        WallJump();



        //misc
        if (!isWallJumping)
        {
            Flip();
        }

    }
    void FixedUpdate()
    {

        //movement

        if (!isOnGround())
        {
            finalaccelrate = AirBrakeaccelRate;
        }
        if (isOnGround())
        {
            //wallJumpTimer = -1f;
            if (movementinput.x != 0)
            {
                finalaccelrate = accelRate;
            }
            if (movementinput.x == 0)
            {
                finalaccelrate = decelRate;
            }
        }

        targetvelocity = (movementinput.x * topSpeed) - (rb.velocity.x);

        if (!isDying) rb.AddForce(finalaccelrate * targetvelocity * Vector2.right, ForceMode2D.Force);






        //jump
        if (!isJumping)
        {
            if (rb.velocity.y < 0)
            {
                finalgravity = gravity * gravitydownscale;
            }
            if (rb.velocity.y >= 0)
            {
                finalgravity = gravity;
            }
            
            
        }

        



        //gravity
        if (!isDying)
        {
            rb.AddForce(Vector3.down * finalgravity, ForceMode2D.Force);
        }

       
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -terminalvelocity, 100000));
    }



    




    public void StartJumpSequence()
    {
        StartCoroutine(RememberJump());
    }
   
    public IEnumerator RememberJump()
    {
        for (int i = 0; i < jumpwaitframes; i++)
        {
            if ((MayJump >= 0f))
            {
                Jump();
                JumpFlag = true;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        while (isJumping)
        {
            while (((jump.action.inProgress) | (rb.velocity.y < transitionpoint)) && (JumpFlag == true))
            {
                if (Mathf.Abs(rb.velocity.y) < SquashAndStretchRANGE)
                {
                    finalgravity = gravity * SquashAndStretchFACTOR;
                }
                if (rb.velocity.y < 0 - SquashAndStretchRANGE)
                {
                    finalgravity = gravity * gravitydownscale;
                }
                if (rb.velocity.y >= 0 + SquashAndStretchRANGE)
                {
                    finalgravity = gravity;
                }
                if (((rb.velocity.y <= 0.001f) && (isOnGround())) | isWallSliding)
                {
                    isJumping = false;
                    yield break;
                }

                yield return 0;
            }
            JumpFlag = false;
            if (rb.velocity.y > transitionpoint)
            {
                finalgravity = gravity * cutoffgravitydownscale;
            }

            if (((rb.velocity.y <= 0.001f) && (isOnGround())) | isWallSliding)
            {
                
                isJumping = false;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    public void Jump()
    {
        if (!isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpspeed);
            isJumping = true;
        }
            
    }
    private void Flip()
    {
        if (isFacingRight && movementinput.x < 0f || !isFacingRight && movementinput.x > 0f)
        {
            isFacingRight = !isFacingRight;
           /* Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;*/
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void WallSlide()
    {
        if (IsWalled() && !isOnGround() && movementinput.x != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isOnGround())
        {
            isWallJumping = false;
            CancelInvoke(nameof(StopWallJumping));
        }
        if (isWallSliding)
        {
            sideinput = true;
            isWallJumping = false;
            wallJumpingDirection = -facingDirection();
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jump.action.triggered && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            sideinput = false;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (facingDirection() != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
          /*      Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;*/
                transform.Rotate(0f, 180f, 0f);
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        sideinput = true;
    }




    void Die(Vector2 p)
    {
        StartCoroutine(deathcoroutine(p));
    }
    public IEnumerator deathcoroutine(Vector2 p)
    {
        isDying = true;
        transform.parent = null;
        float timer = respawnDelaySeconds;
        
        while(timer > 0)
        {
            rb.velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().Sleep();
            timer -= Time.deltaTime;
            
            yield return 0;
        }
        GetComponent<Rigidbody2D>().WakeUp();
        rb.position = p;
        isDying = false;
        yield break;
    }
}
