using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Local Properties
    private int totalLives = 3;

    private Vector3 initialPlayerPosition;
    private Vector3 lastCheckPoint;

    private float elapsedTime;
    private float levelStartTime;
    private float easyBestTime;
    private float normalBestTime;
    private float hardBestTime;

    private KeysSpawner key;


    //Settings Variables
    private int difficultyIndex;
    private int movementOptionsIndex;

    [Header("Public Properties")]
    //Keys
    public int totalKeys { get; private set; }
    public int uncollectedKeys { get; private set; }
    //Rows and Columns
    public int rows { get; private set; }
    public int columns { get; private set; }


    //References

    [Header("GameObject References")]
    [SerializeField] private GameObject controlButton;
    [SerializeField] private GameObject joyStick;

    [Header("Class References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private LevelLoader levelLoader;



    //Buttons
    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button restartButton;

    //Sliders
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private HorizontalSelector movementHorizontalSelector;

    //SFX
    [Header("Sound Effects")]
    [SerializeField] private AudioClip winLevelClip; 
    [SerializeField] private AudioClip LoseLevelClip;
    [SerializeField] private AudioClip livePickupClip;
    [SerializeField] private AudioClip keyPickupClip;
    [SerializeField] private AudioClip playerDiedClip;
    [SerializeField] private AudioClip savedLocationClip;






    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.LoadSettings();
        }

        difficultyIndex = (SettingsManager.Instance != null) ? SettingsManager.Instance.difficulty : 0;
        movementOptionsIndex = (SettingsManager.Instance != null) ? SettingsManager.Instance.movementMode : 0;
        easyBestTime =  SettingsManager.Instance.easyBestTime;
        normalBestTime= SettingsManager.Instance.normalBestTime;
        hardBestTime = SettingsManager.Instance.hardBestTime;

        SetRowsAndColumns();
        SetMovementOption();

        initialPlayerPosition = playerMovement.GetPlayerPosition();
        lastCheckPoint = initialPlayerPosition;

        levelStartTime = Time.time;

        key = FindFirstObjectByType<KeysSpawner>();
        SetTotalKeys(key.GetTotalKeys());
        UIManager.Instance.UpdateUIKeys();


        LoadPlayerPrefsSettings();

        //Slider listeners
        if (musicSlider) musicSlider.onValueChanged.AddListener(SetMusicVolumeLive);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(SetSFXVolumeLive);
    

        //Buttons Listeners
        pauseButton.onClick.AddListener(GamePaused);
        resumeButton.onClick.AddListener(GameResumed);
        optionsButton.onClick.AddListener(ShowOptionsPanel);
        restartButton.onClick.AddListener(Restart);


        Time.timeScale = 1;

    }

    private void SetSFXVolumeLive(float value)
    {
        UIManager.Instance.sfxSliderValue.text = sfxSlider.value.ToString("F1");
        AudioManager.Instance.UpdateSFXVolumesLive(value);
    }

    private void SetMusicVolumeLive(float value)
    {
        UIManager.Instance.musicSliderValue.text = musicSlider.value.ToString("F1");
        AudioManager.Instance.UpdateMusicVolumesLive(value);
    }

    private void SetRowsAndColumns()
    {
        //Easy Mode
        if (difficultyIndex == 0)
        {
            rows = 10;
            columns = 10;
        }

        else if (difficultyIndex == 1) 
        {
            rows = 20;
            columns = 20;
        }

        else if(difficultyIndex == 2)
        {
            rows = 30;
            columns = 30;
        }
    }

    private void SetMovementOption()
    {
        //JoyStick
        if ( movementOptionsIndex == 2)
        {
            joyStick.SetActive(true);
            controlButton.SetActive(false);
        }

        //Control Button
        else if (movementOptionsIndex == 3)
        {
            joyStick.SetActive(false);
            controlButton.SetActive(true);
        }
        if (SettingsManager.Instance) SettingsManager.Instance.SaveSettings();
    }

    public void KeyCollected()
    {
        uncollectedKeys -= 1;
        UIManager.Instance.UpdateUIKeys();
        AudioManager.Instance.PlaySFX(keyPickupClip);
        if (uncollectedKeys == 0)
        {
            mazeGenerator.ActivateEndCellDoor();
        }

    }


    public void UpdateRespawnPoint(Vector3 position)
    {
        AudioManager.Instance.PlaySFX(savedLocationClip);
        lastCheckPoint = position;
        
    }

    private void SetTotalKeys(int totalKeys)
    {
        this.totalKeys = totalKeys;
        uncollectedKeys = totalKeys;
    }



    public void AddLive()
    {
        if (totalLives < 5)
        {
            AudioManager.Instance.PlaySFX(livePickupClip);
            totalLives += 1;

            UIManager.Instance.UpdateUILives(false);
        }

    }

    private void SubtractLive()
    {
        totalLives -= 1;
        UIManager.Instance.UpdateUILives(true);

    }

    public void PlayerDied()
    {
        AudioManager.Instance.PlaySFX(playerDiedClip);
        if (totalLives > 0)
        {
            ReSpawn(lastCheckPoint);
            SubtractLive();
        }
        else if (totalLives == 0)
        {
            //gameover
            GameOver();
        }
    }
    private void ReSpawn(Vector3 reSpawnPoint)
    {
        playerMovement.SetPlayerPosition(reSpawnPoint);
    }

    public float UpdateTime()
    {
        elapsedTime = Time.time - levelStartTime;
        return elapsedTime;
    }

    private void SetBestTime()
    {
        //easy mode
        //    1:20          00:00
        if ((easyBestTime == 0 || elapsedTime < GetBestTime()) && difficultyIndex == 0)
        {
            easyBestTime = elapsedTime;
            SettingsManager.Instance.easyBestTime = easyBestTime;
        }

        //normal mode
        if ((normalBestTime == 0 || elapsedTime < GetBestTime()) && difficultyIndex == 1)
        {
            normalBestTime = elapsedTime;
            SettingsManager.Instance.normalBestTime = normalBestTime;
        }

        //hard mode
        if ((hardBestTime == 0 || elapsedTime < GetBestTime()) && difficultyIndex == 2)
        {
            hardBestTime = elapsedTime;
            SettingsManager.Instance.hardBestTime = hardBestTime;
        }
        SettingsManager.Instance.SaveSettings();
    }

    public float GetBestTime()
    {
        
        if (difficultyIndex == 0)
        {
            return easyBestTime;
        }
        else if (difficultyIndex == 1)
        {
            return normalBestTime;
        }
        else
        {
            return hardBestTime;
        }
    }
    public void WinLevel()
    {

        AudioManager.Instance.PlaySFX(winLevelClip);
        Time.timeScale = 0;
        SetBestTime();
        UIManager.Instance.UpdateWinPanelStat();
        UIManager.Instance.winPanel.SetActive(true);

    }

    private void GameOver()
    {
        AudioManager.Instance.PlaySFX(LoseLevelClip);

        Time.timeScale = 0;
        UIManager.Instance.UpdateLoseLevelStat();
        UIManager.Instance.gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        mazeGenerator.ClearMaze();
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    private void GamePaused()
    {
        UIManager.Instance.pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void GameResumed()
    {
        UIManager.Instance.pausePanel.SetActive(false);
        Time.timeScale = 1;
        SettingsManager.Instance.SaveSettings();
    }

    private void ShowOptionsPanel()
    {
        UIManager.Instance.optionsPanel.SetActive(true);
        UIManager.Instance.pausePanel.SetActive(false);
    }

    public void SaveAndClose(GameObject panel)
    {
        panel.SetActive(false);
        UIManager.Instance.pausePanel.SetActive(true);

        movementOptionsIndex = movementHorizontalSelector.currentIndex;
        SetMovementOption();

        if (SettingsManager.Instance != null)
        {
            SettingsManager.Instance.musicVolume = musicSlider.value;
            SettingsManager.Instance.sfxVolume = sfxSlider.value;
            SettingsManager.Instance.movementMode = movementHorizontalSelector.currentIndex;

            SettingsManager.Instance.SaveSettings();
        }

    }

    public void BackToMainMenuButton(GameObject panel)
    {
        panel.SetActive(false);
        levelLoader.LoadLevel(0);
        SettingsManager.Instance.SaveSettings();
    }

    private void LoadPlayerPrefsSettings()
    {
        //Load new settings at start
        if (musicSlider) musicSlider.value = SettingsManager.Instance.musicVolume;
        if (sfxSlider) sfxSlider.value = SettingsManager.Instance.sfxVolume;
        if (movementHorizontalSelector) movementHorizontalSelector.currentIndex = SettingsManager.Instance.movementMode;

        UIManager.Instance.musicSliderValue.text = musicSlider.value.ToString("F1");
        UIManager.Instance.sfxSliderValue.text = sfxSlider.value.ToString("F1");
        movementHorizontalSelector.UpdateDisplay();
    }

}
