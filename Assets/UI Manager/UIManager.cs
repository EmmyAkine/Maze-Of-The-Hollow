using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //lives
    [Header("Lives Section")]
    [SerializeField] private GameObject[] lives;
    [SerializeField] private Sprite usedLiveSprite;
    [SerializeField] private Sprite unUsedLiveSprite;


    //Texts
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI uiKeys;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI winLevelStatText;
    [SerializeField] private TextMeshProUGUI loseLevelStatText;
    public TextMeshProUGUI musicSliderValue; 
    public TextMeshProUGUI sfxSliderValue;


    //Panels
    [Header("Panel References")]
    public GameObject pausePanel; 
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject optionsPanel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateUIKeys();


    }

    private void Update()
    {
        UpdateUITimer();
    }

    public void UpdateUILives(bool liveUsed)
    { 
       if (liveUsed)
       {
            OnLiveUsed();

       }
        else
        {
            OnLiveGained();

        }
    }
    private void OnLiveUsed()
    {


        for (int i = lives.Length - 1; i >= 0; i--)
        {
            if (usedLiveSprite == null || unUsedLiveSprite == null || lives == null) break;

            if (lives[i].CompareTag("UnUsedLive"))
            {
                lives[i].GetComponent<Image>().sprite = usedLiveSprite;
                lives[i].tag = "UsedLive";
                break;
            }
        }
    }




    private void OnLiveGained()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (usedLiveSprite == null || usedLiveSprite == null || lives == null) break;

            if (lives[i].CompareTag("UsedLive"))
            {
                lives[i].GetComponent<Image>().sprite = unUsedLiveSprite;
                lives[i].tag = "UnUsedLive";
                break;
            }
        }
    }

    public void UpdateUIKeys()
    {
        uiKeys.text = CollectedKeys();
    }

    private void UpdateUITimer() //Score
    {
        float elapsedTime = GameManager.Instance.UpdateTime();
        timerText.text = ConvertTimeFormat(elapsedTime);
    }

    public void UpdateWinPanelStat()
    {
        float elapsedTime = GameManager.Instance.UpdateTime();
        float bestTime = GameManager.Instance.GetBestTime();
        string elapsedTimeString = ConvertTimeFormat(elapsedTime);
        string bestTimeString = ConvertTimeFormat(bestTime);
        winLevelStatText.text = $"Time: {elapsedTimeString} \r\nBest Time: {bestTimeString}";
    }

    public void UpdateLoseLevelStat()
    {
        float elapsedTime = GameManager.Instance.UpdateTime();
        string elapsedTimeString = ConvertTimeFormat(elapsedTime);
        string keysCollected = CollectedKeys();

        loseLevelStatText.text = $"Out of lives!\r\nKeys collected: {keysCollected}\r\nTime: {elapsedTimeString}\r\n";


    }

    private string CollectedKeys()
    {
        string collectedKeys = (GameManager.Instance.totalKeys - GameManager.Instance.uncollectedKeys) + "/" + GameManager.Instance.totalKeys;
        return collectedKeys;
    }

    private string ConvertTimeFormat(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        string text = string.Format("{0:00}:{1:00}", minutes, seconds);
        return text;
    }


}
