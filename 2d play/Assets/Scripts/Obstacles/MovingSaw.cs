using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    public AnimationCurve movingCurve;
    public List<Transform> WayPoints;
    public float timeScale;
    public float defaultProgress;
    float slider;
    int moveDir;
    float progress;
    int WaypointNumber;
    float fullDistance;
    public bool looped;
    // Start is called before the first frame update
    void Start()
    {
        slider = defaultProgress;
        moveDir = 1;
    }
    void Update()
    {
        if (!looped)
        {
            if (moveDir == 1)
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
            }
           
        }
        if (looped)
        {
            if (moveDir == 1)
            {
                if (slider < 1f)
                {
                    slider += timeScale * Time.deltaTime;
                }
                else slider = 0;
            }
         
        }
        //Debug.Log(slider);
        // transform.position = Vector2.Lerp(WayPoints[0].position, WayPoints[1].position, movingCurve.Evaluate(Mathf.Clamp01(slider)));
        //Debug.Log(WayPoints.Count);
        progress = (movingCurve.Evaluate(slider) * (WayPoints.Count - 1)); ///slider * (WayPoints.Count-1);
        WaypointNumber = Mathf.FloorToInt(Mathf.Clamp(progress, 0, WayPoints.Count - 1.001f));
        //fullDistance = Vector2.Distance(WayPoints[WaypointNumber].position, WayPoints[WaypointNumber + 1].position);
        gameObject.transform.position = Vector2.Lerp(WayPoints[WaypointNumber].position, WayPoints[WaypointNumber + 1].position, (Mathf.Clamp(progress, 0, WayPoints.Count - 1) - WaypointNumber));

    }
}
