using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class DialogBase : MonoBehaviour
{
    [SerializeField] private string title;
    public List<UIElementBase> elements;
    public UnityEvent onOpen;
    public string Title
    {
        get
        {
            if (title == null || title == string.Empty)
            {
                title = gameObject.name.ToUpper();
            }
            return title;
        }
    }

    public virtual void OnEnable()
    {
        onOpen?.Invoke();
    }
    public abstract void SetValues();

    public abstract void SetValuesOnSceneLoad();
}
