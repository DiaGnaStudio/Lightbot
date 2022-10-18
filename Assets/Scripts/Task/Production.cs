using UnityEngine;

[CreateAssetMenu(fileName = "New Production Task", menuName = "Task/Production", order = 5)]
public class Production : TaskBase
{
    public override bool Run(Transform characterTransform)
    {
        return true;
    }

    public void AddTask(TaskBase taskBase)
    {

    }
}