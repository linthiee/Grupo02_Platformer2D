using UnityEngine;
using UnityEngine.SceneManagement;
using static EventBus;

public class GameMenuManager : MonoBehaviour
{
    private IEventBus _eventBus;

    private void Start()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<EasyLevelEvent>(OnEasyLevelSelected);

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<EasyLevelEvent>(OnEasyLevelSelected);
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
        }
    }

    private void OnEasyLevelSelected(EasyLevelEvent eventData)
    {
        SceneManager.LoadScene("LevelEasy");
    }

    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        SceneManager.LoadScene("MainMenu");
    }
}