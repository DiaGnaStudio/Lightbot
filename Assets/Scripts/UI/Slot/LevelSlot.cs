using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlot : MonoBehaviour
{
    [SerializeField] private GameObject lockObject;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color passedColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private TMP_Text numberText;

    private Button button;

    [EasyButtons.Button]
    public void SetValue(SlotMode slotMode, int index)
    {
        button = GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.RemoveAllListeners();

        lockObject.SetActive(false);

        numberText.SetText((index+1).ToString());

        switch (slotMode)
        {
            case SlotMode.Locked:
                lockObject.SetActive(true);
                backgroundImage.color = lockedColor;
                break;
            case SlotMode.Passed:
                backgroundImage.color = passedColor;
                button.interactable = true;
                button.onClick.AddListener(() =>
                {
                    if (LevelManager.GetActiveIndex() == index)
                    {
                        GameManager.instance.State = GameManager.GameState.StageStarted;
                    }
                    else
                    {
                        LevelManager.LoadScene(index);
                    }
                });
                break;
            case SlotMode.Current:
                backgroundImage.color = activeColor;
                button.interactable = true;
                button.onClick.AddListener(() =>
                {
                    GameManager.instance.State = GameManager.GameState.StageStarted;
                });
                break;
        }
    }

    public enum SlotMode
    {
        Locked,
        Passed,
        Current
    }
}