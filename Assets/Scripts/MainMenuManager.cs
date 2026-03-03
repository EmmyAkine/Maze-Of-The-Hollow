using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    // Button references
    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button highScoresButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button aboutButton;
    [SerializeField] private Button quitButton;

    // Panel references
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject highScoresPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject aboutPanel;

    //Text field
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI musicSliderValue;
    [SerializeField] private TextMeshProUGUI SFXSliderValue;
    [SerializeField] private TextMeshProUGUI highScoreText;

    //Sliders
    [Header("Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private HorizontalSelector difficultyHorizontalSelector, movementHorizontalSelector;

    [Header("Reference")]
    [SerializeField] private LevelLoader levelLoader;

    private void Start()
    {
        //Button Listeners
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        highScoresButton.onClick.AddListener(OpenHighScore);
        creditsButton.onClick.AddListener(OpenCredits);
        aboutButton.onClick.AddListener(OpenAbout);
        quitButton.onClick.AddListener(QuitGame);


        //Slider listeners
        if (musicSlider) musicSlider.onValueChanged.AddListener(SetMusicVolumeLive);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(SetSFXVolumeLive);

        LoadPlayerPrefsSettings();


        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    private void OnEnable()
    {
    }

    private void StartGame()
    {
        levelLoader.LoadLevel(1);
    }

    private void OpenOptions()
    {
        optionsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    private void OpenHighScore()
    {
        highScoresPanel.SetActive(true);
        UpdateHighScoreText();
        mainMenuPanel.SetActive(false);


    }
    private void OpenCredits()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

    }

    private void OpenAbout()
    {
        aboutPanel.SetActive(true);
        mainMenuPanel.SetActive(false);

    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        mainMenuPanel.SetActive(true);

    }

    public void SaveAndClose(GameObject panel)
    {
        SettingsManager.Instance.musicVolume = musicSlider.value;
        SettingsManager.Instance.sfxVolume = sfxSlider.value;
        SettingsManager.Instance.movementMode = movementHorizontalSelector.currentIndex;
        SettingsManager.Instance.difficulty = difficultyHorizontalSelector.currentIndex;

        SettingsManager.Instance.SaveSettings();
        panel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }


    private void SetMusicVolumeLive(float value)
    {
        musicSliderValue.text = musicSlider.value.ToString("F1");
        AudioManager.Instance.UpdateMusicVolumesLive(value);
    }

    private void SetSFXVolumeLive(float value)
    {
        SFXSliderValue.text = sfxSlider.value.ToString("F1");
        AudioManager.Instance.UpdateSFXVolumesLive(value);
    }

    private void LoadPlayerPrefsSettings()
    {
        SettingsManager.Instance.LoadSettings();

        //Load new settings at start
        if (musicSlider) musicSlider.value = SettingsManager.Instance.musicVolume;
        if (sfxSlider) sfxSlider.value = SettingsManager.Instance.sfxVolume;
        if (difficultyHorizontalSelector) difficultyHorizontalSelector.currentIndex = SettingsManager.Instance.difficulty;
        if (movementHorizontalSelector) movementHorizontalSelector.currentIndex = SettingsManager.Instance.movementMode;

        musicSliderValue.text = musicSlider.value.ToString("F1");
        SFXSliderValue.text = sfxSlider.value.ToString("F1");
        difficultyHorizontalSelector.UpdateDisplay();
        movementHorizontalSelector.UpdateDisplay();
    }
    private void UpdateHighScoreText()
    {
        string easyBestTimeText = ConvertTimeFormat(SettingsManager.Instance.easyBestTime);
        string normalBestTimeText = ConvertTimeFormat(SettingsManager.Instance.normalBestTime);
        string hardBestTimeText = ConvertTimeFormat(SettingsManager.Instance.hardBestTime);
        highScoreText.text = $"YOUR HIGH SCORE:\r\nEASY MODE: {easyBestTimeText} \r\nNORMAL MODE: {normalBestTimeText} \r\nHARD MODE: {hardBestTimeText}";
    }
    private string ConvertTimeFormat(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        string text = string.Format("{0:00}:{1:00}", minutes, seconds);
        return text;
    }
}
