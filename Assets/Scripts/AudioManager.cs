using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Sound Effects Clips")]
    [SerializeField] private AudioClip keyCollectClip;
    [SerializeField] private AudioClip lifeGainClip;
    [SerializeField] private AudioClip ghostCatchClip;
    [SerializeField] private AudioClip playerDieClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip checkpointActivateClip;
    [SerializeField] private AudioClip OnPauseClip;





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
    private void Start()
    {
        musicSource.loop = true;

        UpdateVolumes();

    }
    public void UpdateVolumes()
    {
        musicSource.volume = SettingsManager.Instance.musicVolume;
        sfxSource.volume = SettingsManager.Instance.sfxVolume;
    }
    public void UpdateMusicVolumesLive(float value)
    {
        musicSource.volume = value;
    }

    public void UpdateSFXVolumesLive(float value)
    {
        sfxSource.volume = value;
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    

}
