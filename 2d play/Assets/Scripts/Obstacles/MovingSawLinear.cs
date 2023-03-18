using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    public AnimationCurve movingCurve;
    public List<Transform> WayPoints;
    float slider;
    int moveDir;
    float progress;
    int WaypointNumber;
    float fullDistance;
    // Start is called before the first frame update
    void Start()
    {
        slider = 0;
        moveDir = 1;
    }
    void Update()
    {
        if (moveDir == 1)
        {
            if (slider < 1.1f)
            {
                slider += Time.deltaTime;
            }
            else moveDir = -1;
        }
        if (moveDir == -1)
        {
            if (slider > -0.1f)
            {
                slider -= Time.deltaTime;
            }
            else moveDir = 1;
        }
        //Debug.Log(slider);
        // transform.position = Vector2.Lerp(WayPoints[0].position, WayPoints[1].position, movingCurve.Evaluate(Mathf.Clamp01(slider)));
        //Debug.Log(WayPoints.Count);
        progress = slider * (WayPoints.Count-1);
        WaypointNumber = Mathf.FloorToInt(Mathf.Clamp(progress, 0, WayPoints.Count-1.001f));
        //fullDistance = Vector2.Distance(WayPoints[WaypointNumber].position, WayPoints[WaypointNumber + 1].position);
        gameObject.transform.position = Vector2.Lerp(WayPoints[WaypointNumber].position, WayPoints[WaypointNumber + 1].position, (Mathf.Clamp(progress, 0, WayPoints.Count-1) - WaypointNumber));
    }
}
