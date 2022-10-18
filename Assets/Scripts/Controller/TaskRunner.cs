using System.Collections.Generic;
using UnityEngine;

public class TaskRunner : MonoBehaviour
{
    public void PlayTask<T>(T taskBase) where T : TaskBase
    {
        var success = taskBase.Run(transform);
        if (!success)
        {
            
        }
    }
}
