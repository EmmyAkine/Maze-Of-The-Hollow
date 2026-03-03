using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Button))]
public class UIButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSound;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
    }
}
