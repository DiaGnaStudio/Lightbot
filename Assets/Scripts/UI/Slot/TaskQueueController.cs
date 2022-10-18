using System.Collections.Generic;
using UnityEngine;

public class TaskQueueController : MonoBehaviour
{
    List<Queue> queues = new List<Queue>();

    private void Start()
    {
        queues = new List<Queue>
        {
            new Queue(new List<TaskBase>()) // main queue
        };
    }

    private void OnDestroy()
    {
        queues = new List<Queue>
        {
            new Queue(new List<TaskBase>()) // main queue
        };
    }

    public void AddOtherQueue(List<TaskBase> tasks)
    {
        queues.Add(new Queue(tasks));
    }

    public void AddTask(TaskBase newTask, int queueIndex = 0)
    {
        queues[queueIndex].Add(newTask);
    }

    public void RemoveTask(TaskBase oldTask, int queueIndex = 0)
    {
        queues[queueIndex].Remove(oldTask);
    }

    public struct Queue
    {
        public List<TaskBase> Tasks { get; private set; }

        private Queue<TaskBase> queue;

        public System.Action<bool> onComplete;
        public System.Action<TaskBase> TaskAdded;
        public System.Action<TaskBase> TaskRemoved;

        public Queue(List<TaskBase> tasks) : this()
        {
            Tasks = tasks;
        }

        public void Add(TaskBase newTask)
        {
            if (Tasks.Count >= 12) return;
            Tasks.Add(newTask);
            TaskAdded?.Invoke(newTask);
        }

        public void Remove(TaskBase oldTask)
        {
            if (Tasks.Contains(oldTask))
            {
                Tasks.Remove(oldTask);
                TaskRemoved?.Invoke(oldTask);
            }
        }

        public TaskBase Peek()
        {
            return queue.Peek();
        }

        public TaskBase Dequeue()
        {
            return queue.Dequeue();
        }

        public void Run()
        {
            if (queue == null)
            {
                queue = new Queue<TaskBase>(Tasks);
            }
            else if (queue.Count == 0)
            {
                onComplete?.Invoke(true);
                return;
            }
            var task = Dequeue();
            task.onComplete = CompleteTask;
            GameManager.instance.TaskRunner.PlayTask(task);
        }

        private void CompleteTask(bool value)
        {
            if (value)
            {
                Run();
            }
            else
            {
                Debug.Log("FAILED!");
                onComplete?.Invoke(false);
                GameManager.instance.State = GameManager.GameState.FailedTasks;
            }
        }
    }

    public Queue GetProductionQueue()
    {
        //TODO: We have a production list for now, this should be changed later
        return queues[1];
    }

    public List<TaskBase> GetProductionTask()
    {
        return GetProductionQueue().Tasks;
    }

    #region RUN QUEUE

    const int mainQueueIndex = 0;

    public void Run()
    {
        var x = queues[mainQueueIndex];
        x.onComplete = CompleteQueue;
        x.Run();
    }

    /// <summary>
    /// call when all queue are complete
    /// </summary>
    /// <param name="value"></param>
    private void CompleteQueue(bool value)
    {
        if (value)
        {
            // success
            Debug.Log("All task done!");
            GameManager.instance.State = GameManager.GameState.CompleteTasks;
        }
        else
        {
            // failed
            Debug.Log("Have a problem!");
            GameManager.instance.State = GameManager.GameState.FailedTasks;
        }
    }

    #endregion
}