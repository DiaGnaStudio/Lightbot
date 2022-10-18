using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Rotate Task", menuName = "Task/Rotate", order = 3)]
public class Rotate : TaskBase
{
    [SerializeField] private float speed;
    [SerializeField] private Diraction diraction;

    public override bool Run(Transform characterTransform)
    {
        var endValue = diraction == Diraction.ToRight ? characterTransform.eulerAngles - new Vector3(0, 90, 0) : characterTransform.eulerAngles + new Vector3(0, 90, 0);
        characterTransform.DORotate(endValue, 1).OnComplete(() =>
        {
            TaskQueueController.CompleteTask();
        });
        return true;
    }

    public enum Diraction
    {
        ToRight,
        ToLeft
    }
}
