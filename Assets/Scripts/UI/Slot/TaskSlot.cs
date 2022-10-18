using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaskSlot : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;

    public void SetButtonAction(System.Action callback)
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => callback?.Invoke());
    }

    public void Init(TaskBase task, int queueIndex = 0)
    {
        _icon.sprite = task.Icon;

        var page = UI_Manager.instance.GetPageOfType<HUDPage>();
        if (queueIndex != -1)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() =>
            {
                GameManager.instance.QueueController.RemoveTask(task, queueIndex);
                Destroy(gameObject);
            });
        }
    }
}
