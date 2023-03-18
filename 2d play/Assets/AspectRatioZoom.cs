using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioZoom : MonoBehaviour
{
    // Set the desired aspect ratio here (e.g. 16:9)
    public float targetAspectWidth = 16f;
    public float targetAspectHeight = 9f;
    float targetAspect;
    // Get the camera component
    private Camera cameras;

    void Start()
    {
        // Get the camera component
        cameras = GetComponent<Camera>();

        // Calculate the target aspect ratio
        targetAspect = targetAspectWidth / targetAspectHeight;

        // Determine the game window's current aspect ratio
        
    }
    void OnGUI()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Current viewport height should be scaled by this amount
        float scaleHeight = windowAspect / targetAspect;

        // If scaled height is greater than current height, zoom and fill
        if (scaleHeight > 1f)
        {
            Rect rect = cameras.rect;
            rect.width = 1f / scaleHeight;
            rect.height = 1f;
            rect.x = (scaleHeight - 1f) / (2f * scaleHeight);
            rect.y = 0f;
            cameras.rect = rect;
        }
    }
}
