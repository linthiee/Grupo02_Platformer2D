using UnityEngine;
using UnityEngine.SceneManagement;
using static EventBus;

public class GameManager : MonoBehaviour
{
    private IEventBus _eventBus;
    
    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;

        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<EndGameEvent>(OnGameEnd);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
            _eventBus.Unsubscribe<EndGameEvent>(OnGameEnd);
        }
        Application.wantsToQuit -= WantsToQuit;
    }

    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        Debug.Log("OnBackToMenu called");
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }

    public void OnGameEnd(EndGameEvent eventData)
    {
        Debug.Log("OnGameEnd called");

#if UNITY_EDITOR
        Time.timeScale = 1f;
        Debug.Log("quitting...");
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
#elif UNITY_WEBGL
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Menu");
#else
        SceneManager.LoadSceneAsync("Menu");
#endif
    }

    private bool WantsToQuit()
    {
        return true;
    }
}
