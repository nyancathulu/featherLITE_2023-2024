using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    public int rate;
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = rate;
    }
}
