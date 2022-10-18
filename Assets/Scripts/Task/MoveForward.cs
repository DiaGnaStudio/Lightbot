using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Move Forward Task", menuName = "Task/Move Forward", order = 1)]
public class MoveForward : TaskBase
{
    [SerializeField] private float speed;

    public override bool Run(Transform characterTransform)
    {
        var currentBlock = GameManager.instance.StageController.CurrentBlock;
        Block nextBlock = null;
        //foreach (var block in GameManager.instance.StageController.Blocks)
        //{
        //    var height = block.transform.position.y - currentBlock.transform.position.y; ;

        //    var diffX = characterTransform.localPosition.x - block.transform.localPosition.x;
        //    var diffZ = characterTransform.localPosition.z - block.transform.localPosition.z;

        //    var heading = block.transform.position - characterTransform.position;
        //    var dot = Vector3.Dot(heading, characterTransform.forward);

        //    if (dot > 0.7f && Mathf.Abs(height) == 0)
        //    {
        //        if (Mathf.Ceil( diffX) == 0 && Mathf.Abs(diffZ) == 1)
        //        {
        //            nextBlock = block;
        //            break;
        //        }

        //        if (Mathf.Ceil(diffZ) == 0 && Mathf.Abs(diffX) == 1)
        //        {
        //            nextBlock = block;
        //            break;
        //        }
        //    }
        //}
        GameManager.instance.StageController.NextBlock(characterTransform, ref nextBlock);

        if (nextBlock != null)
        {
            var endValue = new Vector3(nextBlock.transform.position.x, characterTransform.position.y, nextBlock.transform.position.z);
            characterTransform.DOMove(endValue, 1).OnComplete(() =>
            {
                TaskQueueController.CompleteTask();
            });
            GameManager.instance.StageController.SuccessMove(nextBlock);
            return true;
        }
        else
        {
            GameManager.instance.StageController.FailMove();
            return false;
        }
    }
}
