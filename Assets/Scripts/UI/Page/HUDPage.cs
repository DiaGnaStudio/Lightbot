using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPage : PageBase
{
    Queue activeQueue = Queue.MainQueue;

    [Header("Task")]
    [SerializeField] private TaskSlot slotPrefab;
    [SerializeField] private Transform taskSlotsParent;
    [Space]
    [SerializeField] private GameObject mainQueue;
    [SerializeField] private Transform mainQueueParent;

    [Space]
    [SerializeField] private GameObject productionQueue;
    [SerializeField] private Transform productionQueueParent;
    
    [Header("Button")]
    [SerializeField] private GameObject nextLevelObject;
    private Button nextLevelButton;

    [Header("Play")]
    [SerializeField] private GameObject playObject;
    private Button playButton;
    private Image playButtonImage;
    [SerializeField] private TMP_Text playButtonText;
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite stopSprite;
    [SerializeField] private Sprite retrySprite;

    public enum Queue
    {
        MainQueue,
        ProductionQueue
    }

    public override void SetValues()
    {
        CreateTaskSlots();
        mainQueueParent.DestroyChildren();
        productionQueueParent.DestroyChildren();

        CheckProductionMode();
    }

    public override void SetValuesOnSceneLoad()
    {
        nextLevelButton = nextLevelObject.GetComponentInChildren<Button>();

        playButton = playObject.GetComponentInChildren<Button>();
        playButtonImage = playObject.GetComponentInChildren<Image>();

        GameManager.instance.OnStateChange += OnChangeGameState;

        SetPlayButtonClick();
        SetNextLevelButtonClick();
    }

    private void CheckProductionMode()
    {
        if (GameManager.instance.StageController.HasProductionTask)
        {
            Debug.Log("Create");
            productionQueue.SetActive(true);

            var newParent = new GameObject("Queue Parent");
            newParent.transform.SetParent(transform);
            var group = newParent.AddComponent<ToggleGroup>();

            productionQueue.transform.SetParent(newParent.transform);
            var productionToggle = productionQueue.AddComponent<Toggle>();
            productionToggle.group = group;
            productionToggle.targetGraphic = productionQueue.GetComponentInChildren<Image>();
            productionToggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    activeQueue = Queue.ProductionQueue;
                }
            });

            mainQueue.transform.SetParent(newParent.transform);
            var mainToggle = mainQueue.AddComponent<Toggle>();
            mainToggle.group = group;
            mainToggle.targetGraphic = mainQueue.GetComponentInChildren<Image>();
            mainToggle.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    activeQueue = Queue.MainQueue;
                }
            });

            mainToggle.isOn = true;

            activeQueue = Queue.MainQueue;
        }
        else
        {
            productionQueue.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.OnStateChange -= OnChangeGameState;
    }

    private void SetPlayButtonClick()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
        {
            switch (GameManager.instance.State)
            {
                case GameManager.GameState.StageStarted:
                    //play mode
                    GameManager.instance.State = GameManager.GameState.Play;
                    break;
                case GameManager.GameState.Play:
                    //stop mode
                    GameManager.instance.State = GameManager.GameState.StageStarted;
                    break;
                case GameManager.GameState.FinishTasks:
                    //retry mode
                    GameManager.instance.State = GameManager.GameState.StageStarted;
                    break;
                case GameManager.GameState.SelectLevel:
                    //none
                    break;
            }

        });
    }

    private void SetNextLevelButtonClick()
    {
        nextLevelButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.AddListener(() =>
        {
            LevelManager.LoadNextLevel();
        });
    }

    private void CreateTaskSlots()
    {
        var tasks = GameManager.instance.StageController.GetAvailableTasks();
        taskSlotsParent.DestroyChildren();
        foreach (var task in tasks)
        {
            var newSlot = Instantiate(slotPrefab, taskSlotsParent);
            newSlot.Init(task, false);
            newSlot.SetButtonAction(() =>
            {
                TaskQueueController.AddTask(task, activeQueue == Queue.MainQueue);
                AddQueueTask(task);
            });
        }
    }

    public void AddQueueTask(TaskBase task)
    {
        Transform parent = activeQueue == Queue.MainQueue ? mainQueueParent : productionQueueParent;
        var newSlot = Instantiate(slotPrefab, parent);
        newSlot.Init(task, true);
        newSlot.SetButtonAction(() =>
        {
            TaskQueueController.RemoveTask(task, activeQueue == Queue.MainQueue);
            Destroy(newSlot.gameObject);
        });
    }

    private void RemoveQueueTask(TaskBase oldTask)
    {

    }

    private void OnChangeGameState(GameManager.GameState state)
    {
        ActiveNextLevelButton(state);
        SetPlayButtonSkin(state);
    }

    private void ActiveNextLevelButton(GameManager.GameState state)
    {
        if (state == GameManager.GameState.FinishTasks && GameManager.instance.StageController.CheckLights())
        {
            nextLevelObject.SetActive(true);
        }
        else
        {
            nextLevelObject.SetActive(false);
        }
    }

    private void SetPlayButtonSkin(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.StageStarted:
                //play mode
                playButtonImage.sprite = playSprite;
                playButtonText.SetText("Play");
                break;
            case GameManager.GameState.Play:
                //stop mode
                playButtonImage.sprite = stopSprite;
                playButtonText.SetText("Stop");
                break;
            case GameManager.GameState.FinishTasks:
                //retry mode
                playButtonImage.sprite = retrySprite;
                playButtonText.SetText("Retry");
                break;
            case GameManager.GameState.SelectLevel:
                //none
                break;
        }
    }
}
