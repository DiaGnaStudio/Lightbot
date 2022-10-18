using System.Collections.Generic;
using UnityEngine;

public class TaskRunner : MonoBehaviour
{
    public void PlayTask<T>(T taskBase) where T : TaskBase
    {
        taskBase.Run(transform);
    }
}
