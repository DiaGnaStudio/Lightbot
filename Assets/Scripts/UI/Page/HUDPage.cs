using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPage : PageBase
{
    private int activeQueueIndex  = 0;
    Dictionary<int, QueuePanel> queuePanels = new Dictionary<int, QueuePanel>();

    [Header("Task")]
    [SerializeField] private TaskSlot slotPrefab;
    [SerializeField] private Transform taskSlotsParent;
    [Space]
    [SerializeField] private QueuePanel queuePanelPrefab;
    //[SerializeField] private GameObject mainQueue;
    [SerializeField] private Transform queuePanelParent;

    //[Space]
    //[SerializeField] private GameObject productionQueue;
    //[SerializeField] private Transform productionQueueParent;

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

    public override void SetValues()
    {
        CreateTaskSlots();
        foreach(var panel in queuePanels.Values)
        {
            panel.Destroy();
        }

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
        queuePanels = new Dictionary<int, QueuePanel>();

        var mainQueuePanel = Instantiate(queuePanelPrefab, queuePanelParent);
        mainQueuePanel.Create(0);

        queuePanels.Add(0, mainQueuePanel);

        if (GameManager.instance.StageController.HasProductionTask)
        {
            Debug.Log("Create");
            //productionQueue.SetActive(true);

            var group = queuePanelParent.gameObject.AddComponent<ToggleGroup>();

            var count = GameManager.instance.StageController.GetAvailableTasks().FindAll(x => x is Production).Count;
            for (int i = 0; i < count; i++)
            {
                var panelIndex = i + 1;
                var newQueuePanel = Instantiate(queuePanelPrefab, transform);
                var toggle = newQueuePanel.Create(panelIndex, queuePanelParent);
                GameManager.instance.QueueController.AddOtherQueue(new List<TaskBase>());

                toggle.onValueChanged.AddListener((value) =>
                {
                    ToggleListener(value, panelIndex);
                });

                queuePanels.Add(panelIndex, newQueuePanel);
            }

            //productionQueue.transform.SetParent(newParent.transform);
            //var productionToggle = productionQueue.AddComponent<Toggle>();
            //productionToggle.group = group;
            //productionToggle.targetGraphic = productionQueue.GetComponentInChildren<Image>();
            //productionToggle.onValueChanged.AddListener((value) =>
            //{
            //    if (value)
            //    {
            //        activeQueue = Queue.ProductionQueue;
            //    }
            //});


            var mainToggle = mainQueuePanel.Create(0, queuePanelParent);
            mainToggle.onValueChanged.AddListener((value) =>
            {
                ToggleListener(value, 0);
            });

            //mainQueue.transform.SetParent(newParent.transform);
            //var mainToggle = mainQueue.AddComponent<Toggle>();
            //mainToggle.group = group;
            //mainToggle.targetGraphic = mainQueue.GetComponentInChildren<Image>();
            //mainToggle.onValueChanged.AddListener((value) =>
            //{
            //    if (value)
            //    {
            //        activeQueue = Queue.MainQueue;
            //    }
            //});

            //mainToggle.isOn = true;
            //mainToggle.isOn = true;
            //activeQueue = Queue.MainQueue;
        }

        void ToggleListener(bool value, int panelIndex)
        {
            if (value)
            {
                activeQueueIndex = panelIndex;
            }
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
                case GameManager.GameState.CompleteTasks:
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
            newSlot.Init(task, -1);
            newSlot.SetButtonAction(() =>
            {
                GameManager.instance.QueueController.AddTask(task, activeQueueIndex);
                AddQueueTask(task);
            });
        }
    }

    public void AddQueueTask(TaskBase task)
    {
        Transform parent = queuePanels[activeQueueIndex].Contant;
        var newSlot = Instantiate(slotPrefab, parent);
        newSlot.Init(task, activeQueueIndex);
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
        if (state == GameManager.GameState.CompleteTasks && GameManager.instance.StageController.CheckLights())
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
            case GameManager.GameState.CompleteTasks:
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
