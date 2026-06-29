using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static EventBus;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject defeatPanel;
    [SerializeField] UnityEngine.Audio.AudioMixer audioMixer;
    
    private IEventBus _eventBus;
    
    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;

        _eventBus = ServiceLoader.GetService<IEventBus>();

        _eventBus.Subscribe<ExitToMenuEvent>(OnBackToMenu);
        _eventBus.Subscribe<EndGameEvent>(OnGameEnd);
        _eventBus.Subscribe<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.Unsubscribe<ExitToMenuEvent>(OnBackToMenu);
            _eventBus.Unsubscribe<EndGameEvent>(OnGameEnd);
            _eventBus.Unsubscribe<PlayerDeathEvent>(OnPlayerDeath);
        }
        Application.wantsToQuit -= WantsToQuit;
    }

    private void OnPlayerDeath(PlayerDeathEvent eventData)
    {
        Time.timeScale = 0f;
        audioMixer.SetFloat("VolumeVFX", -80f);

        defeatPanel.SetActive(true);
    } 
    
    private void OnBackToMenu(ExitToMenuEvent eventData)
    {
        Debug.Log("OnBackToMenu called");
        Time.timeScale = 1f;
        audioMixer.SetFloat("VolumeVFX", 0f);
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
