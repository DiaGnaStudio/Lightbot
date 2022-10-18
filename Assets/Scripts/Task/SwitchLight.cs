using UnityEngine;

[CreateAssetMenu(fileName = "New Light Switch Task", menuName = "Task/Light Switch", order = 4)]
public class SwitchLight : TaskBase
{
    //check target, if light point, turn on and return true, else return false
    public override bool Run(Transform characterTransform)
    {
        var currentBlock = GameManager.instance.StageController.CurrentBlock;

        if (currentBlock is LightBlock lightBlock)
        {
            lightBlock.SwitchState();
            OnComplete(true);
            //TaskQueue.CompleteTask();
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
        
    }
}
