using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Move Forward Task", menuName = "Task/Move Forward", order = 1)]
public class MoveForward : TaskBase
{
    [SerializeField] private float speed;
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> tweener;
    public override bool Run(Transform characterTransform)
    {
        var currentBlock = GameManager.instance.StageController.CurrentBlock;
        Block nextBlock = null;
        
        GameManager.instance.StageController.NextBlock(characterTransform, ref nextBlock);

        if (nextBlock != null)
        {
            var endValue = new Vector3(nextBlock.transform.position.x, characterTransform.position.y, nextBlock.transform.position.z);
            var duraction = CalculateDuraction(characterTransform, endValue, speed);
            tweener = characterTransform.DOMove(endValue, duraction).OnComplete(() =>
            {
                OnComplete(true);
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
