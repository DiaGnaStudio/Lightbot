using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueuePanel : MonoBehaviour
{
    [SerializeField] private Transform contant;
    [SerializeField] private TMP_Text nameText;

    public Transform Contant => contant;

    public Toggle Create(int index = 0, Transform parent = null)
    {
        nameText.SetText(index == 0 ? "Main" : $"Production {index}");
        if (parent == null) return null;
        transform.SetParent(parent);
        var toggle = gameObject.AddComponent<Toggle>();
        toggle.group = parent.GetComponent<ToggleGroup>();
        toggle.targetGraphic = GetComponentInChildren<Image>();
        return toggle;
    }

    public void Destroy()
    {
        transform.DestroyChildren();
    }
}