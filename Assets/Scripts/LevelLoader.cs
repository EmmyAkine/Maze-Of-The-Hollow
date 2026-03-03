using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    private bool hasMainMenu => mainMenu != null;
    public void LoadLevel(int sceneIndex)
    {
        SetMainMenuNull();
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(sceneIndex));
    }

    IEnumerator LoadLevelAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progressValue;

            yield return null;
        }
    }

    private void SetMainMenuNull()
    {
        if (!hasMainMenu)
        {
            return;
        }

        mainMenu.SetActive(false);
    }
}
