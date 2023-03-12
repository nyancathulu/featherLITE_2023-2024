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
    // [SerializeField]
    [Range(0.1f,1)]
    public float accelRate;
    [Range(0.1f, 1)]
    public float decelRate;
    public float targetvelocity;
    public float jumpspeed;
    public float groundcheckdistance;
    public float gravity;
    [Range(1,30)]
    public float wallSlidingSpeed;
    [Range(1, 2)]
    public float gravitydownscale;
    [Range(1,10)]
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
    [Range(0.1f,1)]
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
    public float icesliderange;
    [Range(0, 1)]
    public float icemovebackfactor;
    public float respawnDelaySeconds;
    [Space(5)]
    [Header("References")]
    public GameObject groundChecker;
    public LayerMask ground;
    public GameObject wallChecker;
    public LayerMask wall;
    public GameObject iceChecker;
    public LayerMask ice;
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
    bool icestart;
    float icedirection;
    bool isDying;
    private bool isOnGround()
    {
        return groundChecker.GetComponent<groundChecker>().isOnGround;
    }
    private bool IsIced()
    {
        return Physics2D.OverlapCircle(iceChecker.transform.position, 0.2f, ice);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallChecker.transform.position, 0.2f, wall); // maybe replace w/ adjustable var
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
    }
    void Update()
    {
        //checks
        //isOnGround = groundChecker.GetComponent<groundChecker>().isOnGround;
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

        //ice
        IceSlide();

        //misc
        if (!isWallJumping)
        {
            Flip();
        }
    }
    void FixedUpdate()
    {

        //movement
        targetvelocity = (movementinput.x * topSpeed) - (rb.velocity.x);
        //logger.Log(accelRate * targetvelocity * Vector2.right);
        if (!isDying) rb.AddForce(finalaccelrate * targetvelocity * Vector2.right, ForceMode2D.Impulse); //if (!isWallJumping) 


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
        if (!isDying) rb.AddForce(Vector3.down * finalgravity, ForceMode2D.Impulse);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, terminalvelocity);
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
        if (!icestart)
        {
            if (isFacingRight && movementinput.x < 0f || !isFacingRight && movementinput.x > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        if (icestart)
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
            if (!IsIced())
            {
                sideinput = true;
                icestart = false;
            }
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


    private void IceSlide()
    {
        //logger.Log(icestart);
        if (IsIced())
        {
            if (!icestart)
            {
                sideinput = false;
                if (rb.velocity.x > -0.01f)
                {
                    icedirection = 1;
                }
                if (rb.velocity.x < -0.01f)
                {
                    icedirection = -1;
                }
                icestart = true;
            }
           
        }
        if (icestart)
        {
            if (icedirection == 1)
            {
                if (movement.action.ReadValue<Vector2>().x >= 0)
                {
                    movementinput = new Vector2(icesliderange, movement.action.ReadValue<Vector2>().y);
                }
                if (movement.action.ReadValue<Vector2>().x < 0)
                {
                    movementinput = new Vector2(icesliderange * icemovebackfactor, movement.action.ReadValue<Vector2>().y);
                }

            }
            if (icedirection == -1)
            {
                if (movement.action.ReadValue<Vector2>().x <= 0)
                {
                    movementinput = new Vector2(-icesliderange, movement.action.ReadValue<Vector2>().y);
                }
                if (movement.action.ReadValue<Vector2>().x > 0)
                {
                    movementinput = new Vector2(-icesliderange * icemovebackfactor, movement.action.ReadValue<Vector2>().y);
                }
            }
        }
    }






































    void Die(Vector2 p)
    {
        StartCoroutine(deathcoroutine(p));
    }
    public IEnumerator deathcoroutine(Vector2 p)
    {
        isDying = true;
        float timer = respawnDelaySeconds;
        
        while(timer > 0)
        {
            rb.velocity = Vector2.zero;
            timer -= Time.deltaTime;
            
            yield return 0;
        }
        
        rb.position = p;
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