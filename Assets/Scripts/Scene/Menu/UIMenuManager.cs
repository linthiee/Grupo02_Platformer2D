using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] private Button buttonPlay;

    [SerializeField] private Button buttonSettings;
    [SerializeField] private Button buttonSettingsExit;

    [SerializeField] private Button buttonCredits;
    [SerializeField] private Button buttonCreditsExit;

    [SerializeField] private Button buttonExit;

    [SerializeField] private Button buttonEasy;
    [SerializeField] private Button buttonMedium;
    [SerializeField] private Button buttonHard;

    [SerializeField] private GameObject panelMain;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelCredits;
    [SerializeField] private GameObject panelDifficulty;

    private IEventBus _eventBus;

    private void Awake()
    {
        _eventBus = ServiceLoader.GetService<IEventBus>();

        buttonPlay.onClick.AddListener(OnPlayClicked);

        buttonSettings.onClick.AddListener(OnSettingsClicked);
        buttonSettingsExit.onClick.AddListener(OnPanelExitClicked);

        buttonCredits.onClick.AddListener(OnCreditsClicked);
        buttonCreditsExit.onClick.AddListener(OnPanelExitClicked);

        buttonExit.onClick.AddListener(OnExitClicked);

        buttonEasy.onClick.AddListener(OnEasyClicked);
        buttonMedium.onClick.AddListener(OnMediumClicked);
        buttonHard.onClick.AddListener(OnHardClicked);

#if UNITY_WEBGL
        if (buttonExit != null)
        {
            buttonExit.gameObject.SetActive(false);
        }
#endif
    }

    private void OnPanelExitClicked()
    {
        panelSettings.SetActive(false);
        panelCredits.SetActive(false);
        panelMain.SetActive(true);
    }

    private void OnPlayClicked()
    {
        panelMain.SetActive(false);
        panelDifficulty.SetActive(true);
    }

    private void OnSettingsClicked()
    {
        panelMain.SetActive(false);
        panelSettings.SetActive(true);
    }

    private void OnCreditsClicked()
    {
        panelMain.SetActive(false);
        panelCredits.SetActive(true);
    }

    private void OnExitClicked()
    {
        Application.Quit();
    }

    //private void OnEasyClicked()
    //{
    //    _eventBus.Publish(new EasyLevelEvent());
    //    gameObject.SetActive(false);
    //}

    //private void OnMediumClicked()
    //{
    //    _eventBus.Publish(new MediumLevelEvent());
    //    gameObject.SetActive(false);
    //}

    //private void OnHardClicked()
    //{
    //    _eventBus.Publish(new HardLevelEvent());
    //    gameObject.SetActive(false);
    //}

    private void OnDestroy()
    {
        buttonPlay.onClick.RemoveAllListeners();
        buttonSettings.onClick.RemoveAllListeners();
        buttonSettingsExit.onClick.RemoveAllListeners();
        buttonCreditsExit.onClick.RemoveAllListeners();
        buttonExit.onClick.RemoveAllListeners();
    }
}