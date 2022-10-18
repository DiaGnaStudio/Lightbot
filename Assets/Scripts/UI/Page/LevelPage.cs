using System.Collections.Generic;
using UnityEngine;

public class LevelPage : PageBase
{
    [SerializeField] private LevelSlot slotPrefab;
    [SerializeField] private Transform slotParent;
    List<LevelSlot> slots = new List<LevelSlot>();

    public override void SetValues()
    {
        LoadLevelSlot();
    }

    public override void SetValuesOnSceneLoad()
    {
        CreateLevelSlot();
    }

    private void LoadLevelSlot()
    {
        var currentIndex = GameManager.instance.SaveManager.CurrentLevel;
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (i < currentIndex)
            {
                //passed
                slot.SetValue(LevelSlot.SlotMode.Passed, i);
            }
            else if (i == currentIndex)
            {
                //current
                slot.SetValue(LevelSlot.SlotMode.Current, i);
            }
            else
            {
                //locked
                slot.SetValue(LevelSlot.SlotMode.Locked, i);
            }
        }
    }

    private void CreateLevelSlot()
    {
        var sceneCount = LevelManager.GetAllSceneCount();
        for (int i = 0; i < sceneCount; i++)
        {
            var slot = Instantiate(slotPrefab, slotParent);
            slots.Add(slot);
        }
    }
}
