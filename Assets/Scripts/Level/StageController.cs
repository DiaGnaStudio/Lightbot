using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] private List<Block> blocks = new List<Block>();
    [SerializeField] private Block startBlock;
    List<LightBlock> lightBlocks = new List<LightBlock>();

    [Tooltip("The tasks you want are available at this stage")]
    [SerializeField] private List<TaskBase> availableTask;

    Quaternion startedRotation;
    public Block CurrentBlock { get; private set; }
    public bool HasProductionTask { get; private set; }

    public void Initialization(Transform character)
    {
        if (startBlock == null) startBlock = blocks[0];

        foreach (var block in blocks)
        {
            if (block is LightBlock light)
            {
                lightBlocks.Add(light);
            }
        }

        HasProductionTask = availableTask.Find(x => x is Production);

        character.position = startBlock.transform.position;
        startedRotation = character.localRotation;
        SuccessMove(startBlock);
        character.SetParent(transform);
    }

    public void NextBlock(Transform characterTransform, ref Block nextBlock, ref float height)
    {
        foreach (var block in blocks)
        {
            height = block.transform.position.y - CurrentBlock.transform.position.y;

            var diffX = CurrentBlock.transform.localPosition.x - block.transform.localPosition.x;
            var diffZ = CurrentBlock.transform.localPosition.z - block.transform.localPosition.z;

            var heading = block.transform.position - characterTransform.position;
            var dot = Vector3.Dot(heading, characterTransform.forward);

            //Debug.Log($"JUMP__ height: {height}", block.gameObject);

            if (dot > 0.7f && Mathf.Abs(height) == 1)
            {
                //Debug.Log($"JUMP__ dot: {dot} diffX: {diffX} diffZ: {diffZ}", block.gameObject);
                if (diffX == 0 && Mathf.Abs(diffZ) == 1)
                {
                    nextBlock = block;
                    break;
                }

                if (diffZ == 0 && Mathf.Abs(diffX) == 1)
                {
                    nextBlock = block;
                    break;
                }
            }
        }
    }

    public void NextBlock(Transform characterTransform, ref Block nextBlock)
    {
        foreach (var block in blocks)
        {
            var height = block.transform.position.y - CurrentBlock.transform.position.y; ;

            var diffX = CurrentBlock.transform.localPosition.x - block.transform.localPosition.x;
            var diffZ = CurrentBlock.transform.localPosition.z - block.transform.localPosition.z;

            var heading = block.transform.position - characterTransform.position;
            var dot = Vector3.Dot(heading, characterTransform.forward);

            if (dot > 0.7f && Mathf.Abs(height) == 0)
            {
                //Debug.Log($"MOVE__ dot: {dot} diffX: {diffX} diffZ: {diffZ}", block.gameObject);
                if (Mathf.Ceil(diffX) == 0 && Mathf.Abs(diffZ) == 1)
                {
                    nextBlock = block;
                    break;
                }

                if (Mathf.Ceil(diffZ) == 0 && Mathf.Abs(diffX) == 1)
                {
                    nextBlock = block;
                    break;
                }
            }
        }
    }

    public void ResetCharacter(Transform character)
    {
        character.position = startBlock.transform.position;
        character.localRotation = startedRotation;
        SuccessMove(startBlock);
    }

    //public void FailMove()
    //{
    //    Debug.Log("Faild move! ");
    //    GameManager.instance.State = GameManager.GameState.CompleteTasks;
    //}

    public void SuccessMove(Block block)
    {
        CurrentBlock = block;
    }

    public bool CheckLights()
    {
        foreach (var block in lightBlocks)
        {
            if (block.CurrentState == LightBlock.State.IsOff)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetLights()
    {
        foreach (var block in lightBlocks)
        {
            if (block.CurrentState == LightBlock.State.IsOn)
            {
                block.SwitchState();
            }
        }
    }

    public List<TaskBase> GetAvailableTasks()
    {
        return availableTask;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < blocks.Count-1; i++)
        {
            Gizmos.DrawLine(blocks[i].transform.position + Vector3.up, blocks[i + 1].transform.position + Vector3.up);
        }
    }
}
