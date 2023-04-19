using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class PlayerMovement : MonoBehaviour
{
    public DebugLogger logger;
    [Space]
    public Rigidbody2D rb;
    [Space(5)]
    [Header("Factors")]
  //  [SerializeField]
    public float topSpeed;
    public float IcetopSpeed;
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
    [Range(0.1f, 2)]
    public float Conveyersliderange;
    [Range(0, 1)]
    public float Conveyermovebackfactor;
    public float respawnDelaySeconds;
   // [Range(0, 1)]
    public float iceAccelRate;
    //[Range(0, 1)]
    public float iceDecelRate;
   // [Range(0, 1)]
    public float iceAirAccelRate;
    public float windedGravity;
    public float maxWindedVelocity;
    [Space(5)]
    [Header("References")]
    public GameObject groundChecker;
    public LayerMask ground;
    public GameObject wallChecker;
    public LayerMask wall;
    public GameObject LAYERChecker;
    public LayerMask Conveyer;
    public LayerMask ConveyerRight;
    public LayerMask ConveyerLeft;
    public LayerMask ice;
    public LayerMask wind;
    public LayerMask windLevel;
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
/*    bool wallSliding;
    float wallJumpTimer;
    float wallJumpDirection;
    float WallJumpCoyoteTimer;
    bool isWallJumping;
    bool startedWall;*/
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    private bool sideinput;
    bool Conveyerstart;
    float Conveyerdirection;
    bool isDying;
    bool startIce;
    bool startWind;
    bool runningWindCoroutine;
    bool startedIceCoroutine;
    float internalTopSpeed;
    //chaching
    groundChecker cachedGroundCheck;

    private bool isOnGround()
    {
        return cachedGroundCheck.isOnGround;
    }
    private bool IsConveyered()
    {
        if (Physics2D.OverlapCircle(LAYERChecker.transform.position, 0.2f, ConveyerRight))
        {
            Conveyerdirection = 1;
        }
        if (Physics2D.OverlapCircle(LAYERChecker.transform.position, 0.2f, ConveyerLeft))
        {
            Conveyerdirection = -1;
        }
        return Physics2D.OverlapCircle(LAYERChecker.transform.position, 0.2f, Conveyer);
       
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallChecker.transform.position, 0.2f, wall); // maybe replace w/ adjustable var
    }
    private bool IsIced()
    {
        return Physics2D.OverlapCircle(LAYERChecker.transform.position, 1f, ice);
    }
    private bool IsWinded()
    {
        return Physics2D.OverlapCircle(LAYERChecker.transform.position, 0.2f, wind);
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
        //isOnGround = groundChecker.GetComponent<groundChecker>().isOnGround;
        
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
        //isOnGround = Physics2D.OverlapCircle(groundChecker.transform.position, groundcheckdistance, ground);
      /*  if ((isJumping) && (rb.velocity.y < 0f) && (isOnGround))
        {
            isJumping = false;
        }*/
        
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
        /*  if (movementinput.x > 0)
          {
              gameObject.transform.localScale = Vector3.one * OGscale;
          }
          if (movementinput.x < 0)
          {
              gameObject.transform.localScale = new Vector3(-OGscale, OGscale, OGscale);
          }*/
        //jump
        if (!isWallSliding)
        {
            if (jump.action.triggered)
            {
                //logger.Log("ahhhhh");
                StartJumpSequence();
                // WallJump();
            }
        }

        /*if (wallSliding)
        {
            if (jump.action.triggered)
            {
                WallJump();
            }
        }*/
        //logger.Log(startedWall);

        //wall slide/jump
        /*   if (startedWall)
           {
               if (IsWalled())
               {
                   WallJumpCoyoteTimer = WallJumpCoyoteTime;
               }
               if (isOnGround()) startedWall = false;
           }
           WallSlide();*/
        WallSlide();
        WallJump();
        //logger.Log(rb.velocity.y);

        //Conveyer
        ConveyerMove();

        //misc
        if (!isWallJumping)
        {
            Flip();
        }

        //Iced
        //logger.Log(IsIced());
        IceMove();
        if (startIce)
        {
            internalTopSpeed = IcetopSpeed;
        }
        if (!startIce)
        {
            internalTopSpeed = topSpeed;
        }
        //Wind
        CheckWind();
    }
    void FixedUpdate()
    {

        //movement
        targetvelocity = (movementinput.x * internalTopSpeed) - (rb.velocity.x);
        //logger.Log(accelRate * targetvelocity * Vector2.right);
        if (!isDying) rb.AddForce(finalaccelrate * targetvelocity * Vector2.right, ForceMode2D.Force); //if (!isWallJumping) 


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
 /*       if (isJumping)
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
        }*/
        



        //gravity
        if (!isDying)
        {
            if (startWind) rb.AddForce(Vector3.down * windedGravity, ForceMode2D.Impulse);
            else rb.AddForce(Vector3.down * finalgravity, ForceMode2D.Force);
           /* if (IsWinded())
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -windGravityLimit, 1000));
                if (Physics2D.OverlapCircle(LAYERChecker.transform.position, 0.2f, windLevel))
                {
                    rb.AddForce(Vector3.up * windUpForce, ForceMode2D.Force);
                    Debug.Log("memes");
                }
                    
                //else rb.AddForce(Vector3.down * finalgravity, ForceMode2D.Impulse);
                
            }*/

        }

        if (startWind) rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxWindedVelocity, 100000));
        else rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -terminalvelocity, 100000)); ///rb.velocity = Vector2.ClampMagnitude(rb.velocity, terminalvelocity); 
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
                if (((rb.velocity.y <= 0.001f) && (isOnGround())) | isWallSliding) // or wallsiding?
                {
                    //logger.Log("ahhh");
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
            //rb.AddForce(jumpspeed * Vector2.up, ForceMode2D.Impulse);
            //logger.Log("jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpspeed);
            isJumping = true;
        }
            
    }
    private void Flip()
    {
        if (!Conveyerstart)
        {
            if (isFacingRight && movementinput.x < 0f || !isFacingRight && movementinput.x > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        if (Conveyerstart)
        {
            if (isFacingRight && movement.action.ReadValue<Vector2>().x < 0f || !isFacingRight && movement.action.ReadValue<Vector2>().x > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
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
            //if (!IsIced()) startIce = false;
            isWallJumping = false;
            CancelInvoke(nameof(StopWallJumping));
        }
        if (isWallSliding)
        {
            sideinput = true;
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jump.action.triggered && wallJumpingCounter > 0f)
        {
            //logger.Log(jump);
            isWallJumping = true;
            sideinput = false;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y); //TIME DELTATIME IT
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        sideinput = true;
    }


    private void ConveyerMove()
    {
        //logger.Log(Conveyerstart);
        if (IsConveyered())
        {
            if (!Conveyerstart)
            {
                sideinput = false;
                /*if (rb.velocity.x > -0.01f)
                {
                    Conveyerdirection = 1;
                }
                if (rb.velocity.x < -0.01f)
                {
                    Conveyerdirection = -1;
                }*/
                Conveyerstart = true;
            }
           
        }
        if (!IsConveyered())
        {
            if (isOnGround() | IsWalled())
            {
                sideinput = true;
                Conveyerstart = false;
            }
        }
        if (Conveyerstart)
        {
            if (Conveyerdirection == 1)
            {
                if (movement.action.ReadValue<Vector2>().x >= 0)
                {
                    movementinput = new Vector2(Conveyersliderange, movement.action.ReadValue<Vector2>().y);
                }
                if (movement.action.ReadValue<Vector2>().x < 0)
                {
                    movementinput = new Vector2(Conveyersliderange * Conveyermovebackfactor, movement.action.ReadValue<Vector2>().y);
                }

            }
            if (Conveyerdirection == -1)
            {
                if (movement.action.ReadValue<Vector2>().x <= 0)
                {
                    movementinput = new Vector2(-Conveyersliderange, movement.action.ReadValue<Vector2>().y);
                }
                if (movement.action.ReadValue<Vector2>().x > 0)
                {
                    movementinput = new Vector2(-Conveyersliderange * Conveyermovebackfactor, movement.action.ReadValue<Vector2>().y);
                }
            }
        }
    }

    private void IceMove()
    {
        if (IsIced())
        {
            startIce = true;
        }
        if (!IsIced())
        {
            // if (isOnGround() | IsWalled()) startIce = false;

            if (!startedIceCoroutine) StartCoroutine(IceEnd(0.4f));
        }
        



        if (startIce)
        {
            if (isOnGround())
            {
                if (movementinput.x != 0)
                {
                    finalaccelrate = iceAccelRate;
                }
                if (movementinput.x == 0)
                {
                    finalaccelrate = iceDecelRate;
                }
            }
            if (!isOnGround())
            {
                finalaccelrate = iceAirAccelRate;
            }
        }
        if (!startIce)
        {
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
        }
    }
    IEnumerator IceEnd(float t)
    {
        float timer = t;
        while (timer > 0)
        {
            if (IsIced()) yield break;
            if (isOnGround() | IsWalled() | rb.velocity.y < -15)
            {
                startIce = false;
                yield break;
            }
            timer -= Time.deltaTime;
            yield return 0;
        }
        startIce = false;
    }




    void CheckWind()
    {
       // logger.Log(startWind);
        if (IsWinded())
        {
            startWind = true;
        }
        if (startWind == true)
        {
            if (isOnGround() | IsWalled())
            {
                if (!runningWindCoroutine) StartCoroutine(EndWindCoroutine(1.3f));
            }
        }
    }

    IEnumerator EndWindCoroutine(float t)
    {
        runningWindCoroutine = true;
        float timer = t;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }
        runningWindCoroutine = false;
        if (isOnGround() | IsWalled()) startWind = false;
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
        startWind = false;
        isDying = false;
        yield break;
    }

    /*  void WallSlide()
      {
          if (IsWalled() && !isOnGround() && (movementinput.x != 0))
          {
              startedWall = true;
              wallSliding = true;
              rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
              MayJump = -1f;
          }
          else
          {
              wallSliding = false;
          }
      }
      void WallJump()
      {
          StartCoroutine(WallJumpSequence());
      }
      IEnumerator WallJumpSequence()
      {
          wallJumpTimer = wallJumpTime;
          wallJumpDirection = -transform.localScale.x;
          for (int i = 0; i < WallJumpBufferFrames; i++)
          {
              if (wallSliding)
              {

                  //logger.Log("hehehehehehgehgea");
                  // movement.action.Disable();
                  rb.AddForce(new Vector2(wallJumpVector.x * wallJumpDirection, wallJumpVector.y), ForceMode2D.Impulse);
                  isWallJumping = true;
                  break;
              }
              yield return new WaitForFixedUpdate();
          }

          while (wallJumpTimer > 0)
          {

              wallJumpTimer -= Time.deltaTime;
              if (wallJumpDirection < 0)
              {
                  movementinput = new Vector2(Mathf.Clamp(movement.action.ReadValue<Vector2>().x, -1, WallJumpMoveBackLenciency), movement.action.ReadValue<Vector2>().y);
              }
              if (wallJumpDirection > 0)
              {
                  movementinput = new Vector2(Mathf.Clamp(movement.action.ReadValue<Vector2>().x, -WallJumpMoveBackLenciency, 1), movement.action.ReadValue<Vector2>().y);
              }
              if (isOnGround())
              {
                  break;
              }
              yield return 0;
          }
          //movement.action.Enable();
          while (true)
          {
              if (isOnGround() | wallSliding)
              {
                  break;
              }
              yield return 0;
          }
          isWallJumping = false;
          yield break;
      }
      */
}
