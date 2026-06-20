using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIGameplay : MonoBehaviour
{
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelDefeat;

    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonExit;

    [SerializeField] private Button buttonSettingsExit;

    [SerializeField] private Button buttonDefeatExit;
    [SerializeField] private Button buttonRetry;

    private IEventBus _eventBus;

    private bool isPaused = false;
    private void Awake()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        buttonSettings.onClick.AddListener(OnSettingsClicked);
        buttonExit.onClick.AddListener(OnExitClicked);

        buttonSettingsExit.onClick.AddListener(OnBackFromSettingsClicked);

        buttonDefeatExit.onClick.AddListener(OnExitClicked);
        buttonRetry.onClick.AddListener(OnRetryClicked);

        panelPause.SetActive(false);
        panelSettings.SetActive(false);
        panelDefeat.SetActive(false);

        //_eventBus.Subscribe<TowerFallEvent>(OnTowerFall);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }
    private void OnDestroy()
    {
        buttonSettings.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
        buttonSettingsExit.onClick.RemoveAllListeners();
        buttonDefeatExit.onClick.RemoveAllListeners();
        buttonRetry.onClick.RemoveAllListeners();

        if (_eventBus != null)
        {
            //_eventBus.Unsubscribe<TowerFallEvent>(OnTowerFall);
        }
    }
    //private void OnTowerFall(TowerFallEvent eventData)
    //{
    //    Time.timeScale = 0;

    //    Debug.Log("tower fall event");
    //    panelDefeat.SetActive(true);
    //}

    private void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        panelPause.SetActive(isPaused);

        if (!isPaused)
        {
            panelSettings.SetActive(false);
        }
    }

    private void OnRetryClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        panelDefeat.SetActive(false);
    }

    private void OnBackFromSettingsClicked()
    {
        panelSettings.SetActive(false);
        panelPause.SetActive(true);
    }

    private void OnExitClicked()
    {
        if (isPaused || panelDefeat.gameObject.activeInHierarchy)
        {
            Time.timeScale = 1.0f;

            _eventBus.Publish(new EndGameEvent());
            _eventBus.Publish(new ExitToMenuEvent());
        }
    }
    private void OnSettingsClicked()
    {
        if (isPaused)
        {
            panelPause.SetActive(false);
            panelSettings.SetActive(true);
        }
    }
}
