using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Rotate Task", menuName = "Task/Rotate", order = 3)]
public class Rotate : TaskBase
{
    [SerializeField] private float speed;
    [SerializeField] private Diraction diraction;
    DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> tweener;
    public override bool Run(Transform characterTransform)
    {
        var endValue = diraction == Diraction.ToRight ? characterTransform.eulerAngles - new Vector3(0, 90, 0) : characterTransform.eulerAngles + new Vector3(0, 90, 0);
        tweener = characterTransform.DORotate(endValue, 1).OnComplete(() =>
        {
            //TaskQueue.CompleteTask();
            OnComplete(true);
        });
        return true;
    }
    public override void Stop()
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
    }

    public enum Diraction
    {
        ToRight,
        ToLeft
    }
}
