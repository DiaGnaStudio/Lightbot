using System.Collections.Generic;
[System.Obsolete]
public static class TaskQueue
{
    private static Queue mainQueue = new Queue(new List<TaskBase>());
    private static Queue productionQueue = new Queue(new List<TaskBase>());

    public struct Queue
    {
        public List<TaskBase> Tasks { get;private set; }

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
    }

    public static void AddTask(TaskBase newTask, bool isMainTask = true)
    {
        if (isMainTask || !GameManager.instance.StageController.HasProductionTask)
        {
            if (mainQueue.Tasks.Count >= 12) return;
            mainQueue.Add(newTask);
        }
        else
        {
            if (productionQueue.Tasks.Count >= 8) return;
            productionQueue.Add(newTask);
        }
    }

    public static void RemoveTask(TaskBase oldTask, bool isMainTask = true)
    {
        if (isMainTask || !GameManager.instance.StageController.HasProductionTask)
        {
            mainQueue.Remove(oldTask);
        }
        else
        {
            productionQueue.Remove(oldTask);
        }
    }

    static int index = 0;
    static int pIndex = 0;
    static TaskBase lastTask;

    public static void PlayTasks()
    {
        if (mainQueue.Tasks.Count == 0)
        {
            return;
        }
        NextTask();
    }

    public static void CompleteTask()
    {
        if (GameManager.instance.State != GameManager.GameState.Play) return;
        if (mainQueue.Tasks.Count <= index)
        {
            GameManager.instance.State = GameManager.GameState.CompleteTasks;
        }
        else
        {
            NextTask();
        }
    }

    private static void NextTask()
    {
        var task = mainQueue.Tasks[index++];
        lastTask = task;
        GameManager.instance.TaskRunner.PlayTask(task);
    }

    public static void ResetIndex()
    {
        index = 0;
    }

    public static void Reset()
    {
        ResetIndex();
        mainQueue.Tasks.Clear();
        productionQueue.Tasks.Clear();
    }
}
