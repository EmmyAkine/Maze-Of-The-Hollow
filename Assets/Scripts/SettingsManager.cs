using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    //Singleton instance
    public static SettingsManager Instance;

    //Sliders


    // Public fields for settings (accessed globally)
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.8f;
    public int movementMode = 0;  // 0=WASD, 1=Arrows
    public int difficulty = 1;
    public float easyBestTime;
    public float normalBestTime;
    public float hardBestTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("MovementMode", movementMode);
        PlayerPrefs.SetInt("Difficulty", difficulty);
        PlayerPrefs.SetFloat("EasyBestTime", easyBestTime);
        PlayerPrefs.SetFloat("NormalBestTime", normalBestTime);
        PlayerPrefs.SetFloat("HardBestTime", hardBestTime);

        PlayerPrefs.Save();
    }
    public void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);
        movementMode = PlayerPrefs.GetInt("MovementMode", default);
        difficulty = PlayerPrefs.GetInt("Difficulty", difficulty);
        easyBestTime = PlayerPrefs.GetFloat("EasyBestTime", default);
        normalBestTime = PlayerPrefs.GetFloat("NormalBestTime", default);
        hardBestTime = PlayerPrefs.GetFloat("HardBestTime", default);
    }
}
