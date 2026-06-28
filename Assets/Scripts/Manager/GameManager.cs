using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using static EventBus;

public class GameManager : MonoBehaviour
{
    [Header("Instance singleton")]
    public static GameManager Instance { get; private set; }

    [Header("Player ingame info")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject defeatPannel;

    private IEventBus _eventBus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

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
        SceneManager.LoadSceneAsync("Menu");
    }

    public void OnGameEnd(EndGameEvent eventData)
    {
        Debug.Log("OnGameEnd called");

#if UNITY_EDITOR
        Time.timeScale = 1f;
        Debug.Log("quitting...");
        SceneManager.LoadSceneAsync("Menu");
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
