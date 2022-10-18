using UnityEngine;

public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (!gameObject.TryGetComponent(out T component))
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component) where T : Component
    {
        var children = gameObject.GetComponentsInChildren<T>();
        if (children.Length > 0)
        {
            component = children[0];
            return true;
        }
        component = null;
        return false;
    }

    public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T[] component) where T : Component
    {
        var children = gameObject.GetComponentsInChildren<T>();
        if (children.Length > 0)
        {
            component = children;
            return true;
        }
        component = null;
        return false;
    }

    public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component) where T : Component
    {
        var children = gameObject.GetComponentsInParent<T>();
        if (children.Length > 0)
        {
            component = children[0];
            return true;
        }
        component = null;
        return false;
    }

    public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T[] component) where T : Component
    {
        component = gameObject.GetComponentsInParent<T>();
        return component.Length > 0;
    }
}
