using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Production Task", menuName = "Task/Production", order = 5)]
public class Production : TaskBase
{
    List<TaskBase> tasks;
    TaskQueueController.Queue queue;

    public override bool Run(Transform characterTransform)
    {
        queue = GameManager.instance.QueueController.GetProductionQueue();


        queue.onComplete = (value) =>
        {
            if(value)
            OnComplete(value);
        };

        queue.Run();

        return true;
    }

    public void AddTasks(List<TaskBase> tasks)
    {
        this.tasks = tasks;
        queue = new TaskQueueController.Queue(tasks);
    }

    public void AddTask(TaskBase task)
    {
        if (tasks == null)
        {
            tasks = new List<TaskBase> { task };
        }
        else
        {
            tasks.Add(task);
        }
        queue = new TaskQueueController.Queue(tasks);
    }

    public void RemoveTask(TaskBase task)
    {
        tasks.Remove(task);
        queue = new TaskQueueController.Queue(tasks);
    }
}