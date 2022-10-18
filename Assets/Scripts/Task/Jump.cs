using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Jump Task", menuName = "Task/Jump", order = 2)]
public class Jump : TaskBase
{
    [SerializeField] private float speed;
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> tweener;
    public override bool Run(Transform characterTransform)
    {
        Block nextBlock = null;
        float height = 0;
        GameManager.instance.StageController.NextBlock(characterTransform, ref nextBlock, ref height);

        if (nextBlock != null)
        {
            var endValue = new Vector3(nextBlock.transform.position.x, characterTransform.position.y + height, nextBlock.transform.position.z);
            var duraction = CalculateDuraction(characterTransform, endValue,speed);
            tweener =  characterTransform.DOMove(endValue, duraction).OnComplete(() =>
            {
                OnComplete(true);
                //TaskQueue.CompleteTask();
            });
            GameManager.instance.StageController.SuccessMove(nextBlock);
            return true;
        }
        else
        {

            OnComplete(false);
            return false;
        }
    }

    public override void Stop()
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
    }
}
