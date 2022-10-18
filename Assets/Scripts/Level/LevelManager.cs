using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static System.Action onChangeScene;

    public static int GetActiveIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static int GetAllSceneCount()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);

        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        
        if (index > GameManager.instance.SaveManager.CurrentLevel)
        {
            // new level unlocked
            GameManager.instance.SaveManager.CurrentLevel = index;
        }
    }

    private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        onChangeScene?.Invoke();
    }

    public static void LoadNextLevel()
    {
        LoadScene(GetActiveIndex() + 1);
    }

    public static void LoadPreviousLevel()
    {
        LoadScene(GetActiveIndex() - 1);
    }
}
