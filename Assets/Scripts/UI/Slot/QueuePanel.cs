using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueuePanel : MonoBehaviour
{
    [SerializeField] private Transform contant;
    [SerializeField] private TMP_Text nameText;

    [Header("Theme")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color unselectedColor;
    Image image;

    public Transform Contant => contant;

    public Toggle Create(int index = 0, Transform parent = null)
    {
        nameText.SetText(index == 0 ? "Main" : $"Production {index}");
        if (parent == null) return null;
        transform.SetParent(parent);
        var toggle = gameObject.AddComponent<Toggle>();
        toggle.group = parent.GetComponent<ToggleGroup>();
        image = GetComponentInChildren<Image>();
        toggle.targetGraphic = image;

        toggle.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                SelectedTheme();
            }
            else
            {
                UnselectedTheme();
            }
        });

        return toggle;
    }

    private void SelectedTheme()
    {
        image.color = selectedColor;
    }

    private void UnselectedTheme()
    {
        image.color = unselectedColor;
    }
}