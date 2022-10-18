using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskBase :ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private Sprite icon;

    public Sprite Icon => icon;

    public System.Action<bool> onComplete;

    public abstract bool Run(Transform characterTransform);

    protected void OnComplete(bool value)
    {
        onComplete?.Invoke(value);
    }
}
