using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSlot : MonoBehaviour
{
    [SerializeField] private GameObject lockObject;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color passedColor;
    [SerializeField] private Color activeColor;

    [SerializeField] private TMP_Text numberText;

    private Button button;

    public void SetValue(SlotMode slotMode, int index)
    {
        button = GetComponentInChildren<Button>();
        button.interactable = false;
        button.onClick.RemoveAllListeners();

        lockObject.SetActive(false);

        numberText.SetText((index + 1).ToString());

        switch (slotMode)
        {
            case SlotMode.Locked:
                LockedCallback();
                break;
            case SlotMode.Passed:
                PassedCallback(index);
                break;
            case SlotMode.Current:
                CurrentCallback();
                break;
        }
    }

    private void LockedCallback()
    {
        lockObject.SetActive(true);
        backgroundImage.color = lockedColor;
    }

    private void PassedCallback(int index)
    {
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
    }

    private void CurrentCallback()
    {
        backgroundImage.color = activeColor;
        button.interactable = true;
        button.onClick.AddListener(() =>
        {
            GameManager.instance.State = GameManager.GameState.StageStarted;
        });
    }

    public enum SlotMode
    {
        Locked,
        Passed,
        Current
    }
}