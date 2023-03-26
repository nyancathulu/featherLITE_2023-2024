using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public AnimationCurve movingCurve;
    public List<Transform> WayPoints;
    public float timeScale;
    float slider;
    int moveDir;
    float progress;
    int WaypointNumber;
    float fullDistance;
    bool runningCoroutine1;
    bool runningCoroutine2;
    [Header("Either -1 or 1")]
    [Range (-1,1)]
    public int defaultSpot;
    //Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        slider = 0;
        moveDir = 1;
        runningCoroutine1 = false;
        runningCoroutine2 = false;
    }
    void OnEnable()
    {
        RespawnManager.OnDeath += moveToDefault;
    }
    void OnDisable()
    {
        RespawnManager.OnDeath -= moveToDefault;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartMove(); //just for testing
        }
       /* if (moveDir == 1)
        {
            if (slider < 1.1f)
            {
                slider += timeScale * Time.deltaTime;
            }
            else moveDir = -1;
        }
        if (moveDir == -1)
        {
            if (slider > -0.1f)
            {
                slider -= timeScale * Time.deltaTime;
            }
            else moveDir = 1;
        }*/
        //Debug.Log(slider);
        // transform.position = Vector2.Lerp(WayPoints[0].position, WayPoints[1].position, movingCurve.Evaluate(Mathf.Clamp01(slider)));
       // Debug.Log(rb.velocity);
       
    }
    void FixedUpdate()
    {
       // Debug.Log(rb.velocity);
        //fullDistance = Vector2.Distance(WayPoints[WaypointNumber].position, WayPoints[WaypointNumber + 1].position);
        progress = (movingCurve.Evaluate(slider) * (WayPoints.Count - 1)); ///slider * (WayPoints.Count-1);
        WaypointNumber = Mathf.FloorToInt(Mathf.Clamp(progress, 0, WayPoints.Count - 1.001f));
       transform.position = Vector2.Lerp(WayPoints[WaypointNumber].position, WayPoints[WaypointNumber + 1].position, (Mathf.Clamp(progress, 0, WayPoints.Count - 1) - WaypointNumber));
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!runningCoroutine) StartCoroutine(MoveObject());
    }*/
    public void StartMove()
    {
        if (!runningCoroutine1)
        {
            if (!runningCoroutine2) StartCoroutine(MoveObject()); //runningCoroutine2 = false;
            //StartCoroutine(MoveObject());
        }
            
            
    }
    public IEnumerator MoveObject()
    {
        runningCoroutine1 = true;

        
        /*if (slider > 0.9)
        {
            moveDir = -1;   
        }*/
        /*if (slider < 0.1)
        {
            moveDir = 1;
        }*/
        while (true)
        {
            if (!runningCoroutine1) yield break;
            /*if (moveDir == -1)
            {
                if (slider <= 0)
                {
                    slider = 0;
                    break;
                }
                slider -= (1/timeScale) * Time.deltaTime;
                yield return 0;
            }*/
            /*if (moveDir == 1)
            {*/
                if (slider >= 1)
                {
                    slider = 1;
                    break;
                }
                slider += (1/timeScale) * Time.deltaTime;
                yield return 0;
            //}
           
        }

        runningCoroutine1 = false;
        yield break;
    }
    public void moveToDefault(Vector2 p)
    {
        if (!runningCoroutine2)
        {
            if (runningCoroutine1) runningCoroutine1 = false;
            StartCoroutine(MoveObjectToDefault(defaultSpot));
        }
            
           
    }
    public IEnumerator MoveObjectToDefault(int t)
    {
        runningCoroutine2 = true;

        moveDir = t;

        while (true)
        {
            if (!runningCoroutine2) yield break;
            if (moveDir == -1)
            {
                if (slider <= 0)
                {
                    slider = 0;
                    break;
                }
                slider -= (1 / timeScale * 3) * Time.deltaTime;
                yield return 0;
            }
            if (moveDir == 1)
            {
                if (slider >= 1)
                {
                    slider = 1;
                    break;
                }
                slider += (1 / timeScale * 3) * Time.deltaTime;
                yield return 0;
            }

        }

        runningCoroutine2 = false;
        yield break;
    }
}
