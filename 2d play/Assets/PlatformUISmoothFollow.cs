using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUISmoothFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform platform;
    public float stepDelta;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, platform.position, stepDelta);
    }
}
