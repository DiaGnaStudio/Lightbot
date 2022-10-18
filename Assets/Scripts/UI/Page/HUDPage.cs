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
    [SerializeField] private Transform queuePanelParent;

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
            Destroy(panel.gameObject);
        }
        CheckProductionMode();
    }

    public override void SetValuesOnSceneLoad()
    {
        nextLevelButton = nextLevelObject.GetComponentInChildren<Button>();

        playButton = playObject.GetComponentInChildren<Button>();
        playButtonImage = playObject.GetComponentInChildren<Image>();

        GameManager.instance.OnStateChange += OnChangeGameState;

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

            var mainToggle = mainQueuePanel.Create(0, queuePanelParent);
            mainToggle.onValueChanged.AddListener((value) =>
            {
                ToggleListener(value, 0);
            });

            mainToggle.isOn = true;
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
                var added = GameManager.instance.QueueController.AddTask(task, activeQueueIndex);
                if (added)
                {
                    AddQueueTask(task);
                }
            });
        }
    }

    private void AddQueueTask(TaskBase task)
    {
        Transform parent = queuePanels[activeQueueIndex].Contant;
        var newSlot = Instantiate(slotPrefab, parent);
        newSlot.Init(task, activeQueueIndex);
    }

    private void OnChangeGameState(GameManager.GameState state)
    {
        ActiveNextLevelButton(state);
        SetPlayButtonSkin(state);
    }

    private void ActiveNextLevelButton(GameManager.GameState state)
    {
        nextLevelObject.SetActive(state == GameManager.GameState.CompleteTasks && GameManager.instance.StageController.CheckLights());
    }

    private void SetPlayButtonSkin(GameManager.GameState state)
    {
        playButton.onClick.RemoveAllListeners();
        switch (state)
        {
            case GameManager.GameState.StageStarted:
            case GameManager.GameState.ResetStage:
                PlayMode();
                break;
            case GameManager.GameState.Play:
                StopMode();
                break;
            case GameManager.GameState.FailedTasks:
            case GameManager.GameState.CompleteTasks:
                RetryMode();
                break;
            case GameManager.GameState.SelectLevel:
            case GameManager.GameState.None:
                //none
                break;
        }
    }

    private void PlayMode()
    {
        playButtonImage.sprite = playSprite;
        playButtonText.SetText("Play");
        playButton.onClick.AddListener(() =>
        {
            GameManager.instance.State = GameManager.GameState.Play;
        });
    }

    private void StopMode()
    {
        playButtonImage.sprite = stopSprite;
        playButtonText.SetText("Stop");
        playButton.onClick.AddListener(() =>
        {
            GameManager.instance.State = GameManager.GameState.ResetStage;
        });
    }

    private void RetryMode()
    {
        playButtonImage.sprite = retrySprite;
        playButtonText.SetText("Retry");
        playButton.onClick.AddListener(() =>
        {
            GameManager.instance.State = GameManager.GameState.ResetStage;
        });
    }
}
