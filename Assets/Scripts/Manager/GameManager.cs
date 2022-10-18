using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Components")]
    [SerializeField] UI_Manager uiManagerPrefab;
    [SerializeField] SaveOrLoadManager saveManager;
    [SerializeField] TaskQueueController queueController;

    public SaveOrLoadManager SaveManager => saveManager;
    public TaskQueueController QueueController => queueController;
    public StageController StageController;
    public TaskRunner TaskRunner { get; private set; }

    [Header("Game State")]
    private GameState lastState = GameState.None;
    public GameState State
    {
        get => lastState;
        set
        {
            HandleLastState(value);
        }
    }

    public Action<GameState> OnStateChange { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            var ui = FindObjectOfType<UI_Manager>();
            ui.Initialization();

            StageController ??= FindObjectOfType<StageController>();
            TaskRunner ??= FindObjectOfType<TaskRunner>();

            StageController.Initialization(TaskRunner.transform);

            State = GameState.SelectLevel;

            LevelManager.onChangeScene += () =>
            {
                var ui = FindObjectOfType<UI_Manager>();
                ui.Initialization();

                StageController = FindObjectOfType<StageController>();
                TaskRunner = FindObjectOfType<TaskRunner>();

                StageController.Initialization(TaskRunner.transform);

                QueueController.ResetController();

                State = GameState.StageStarted;
            };

            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void HandleLastState(GameState state)
    {
        if (lastState == state) return;

        switch (state)
        {
            case GameState.SelectLevel:
                UI_Manager.instance.OpenPage(UI_Manager.instance.GetPageOfType<LevelPage>());
                break;
            case GameState.StageStarted:
                if (lastState != GameState.Play)
                {
                    UI_Manager.instance.OpenPage(UI_Manager.instance.GetPageOfType<HUDPage>());
                }
                break;
            case GameState.Play:
                lastState = state;
                QueueController.Run();
                break;
            case GameState.CompleteTasks:
                break;
            case GameState.FailedTasks:
                break;
            case GameState.None:
                break;
            case GameState.ResetStage:
                StageController.ResetLights();
                StageController.ResetCharacter(TaskRunner.transform);
                QueueController.Stop();
                break;
        }

        lastState = state;
        Debug.Log(lastState);
        OnStateChange?.Invoke(lastState);
    }


    public enum GameState
    {
        None,
        SelectLevel,
        StageStarted,
        Play,
        CompleteTasks,
        FailedTasks,
        ResetStage
    }
}