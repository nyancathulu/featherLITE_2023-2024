using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public bool _showLogs;
    public void Log(object message)
    {
        if (_showLogs)
        {
            Debug.Log(message);
        }
    }
}
