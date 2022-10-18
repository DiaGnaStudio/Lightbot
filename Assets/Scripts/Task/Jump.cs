using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New Jump Task", menuName = "Task/Jump", order = 2)]
public class Jump : TaskBase
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;

    public override bool Run(Transform characterTransform)
    {
        Block nextBlock = null;
        float height = 0;
        GameManager.instance.StageController.NextBlock(characterTransform, ref nextBlock, ref height);

        if (nextBlock != null)
        {
            var endValue = new Vector3(nextBlock.transform.position.x, characterTransform.position.y + height, nextBlock.transform.position.z);
            characterTransform.DOMove(endValue, 1).OnComplete(() =>
            {
                TaskQueueController.CompleteTask();
            });
            GameManager.instance.StageController.SuccessMove(nextBlock);
            return true;
        }

        GameManager.instance.StageController.FailMove();
        return false;
    }
}
