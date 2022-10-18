using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskBase : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite icon;

    public Sprite Icon => icon;

    public System.Action<bool> onComplete;

    public abstract bool Run(Transform characterTransform);
    public abstract void Stop();

    protected void OnComplete(bool value)
    {
        onComplete?.Invoke(value);
    }

    protected float CalculateDuraction(Transform characterTransform, Vector3 endValue, float speed)
    {
        return Vector3.Distance(characterTransform.position, endValue) / (speed == 0 ? 1 : speed);
    }
}
