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

    public void Init(TaskBase task, bool inTaskQueue = false)
    {
        _icon.sprite = task.Icon;

        //_button.onClick.RemoveAllListeners();
        //_button.onClick.AddListener(() =>
        //{
        //    if (inTaskQueue)
        //    {
        //        //remove from task queue
        //        TaskQueueController.RemoveTask(task);
        //        Destroy(gameObject);
        //    }
        //    else
        //    {
        //        //add to task queue
        //        TaskQueueController.AddTask(task);
        //    }
        //});
    }
}
